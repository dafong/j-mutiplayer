require "mechanize"
require "sqlite3"
require "thread"

module TaskStatus
	RUNNING = 1
	FINISHED= 2
	TIMEOUT = 4
end

class ConArray
	def initialize()
		@list  = []
		@mutex = Mutex.new
		@cond  = ConditionVariable.new
	end

	def take
		@mutex.synchronize do
			@list.shift
		end
	end

	def add(item)
		return if item.nil?
		@mutex.synchronize do
			@list << item
			@cond.signal
		end
	end

	def peek
		@list.first
	end

end

class ThreadPool
	class Task
		attr_reader :start_at, :exception, :status
		def initialize(pool, *args, &block)
			@pool = pool
			@args = args
			@block= block
		end

		def execute(thread)
			@status = TaskStatus::RUNNING

			begin
				@block.call(*@args)
			rescue Exception => e
				@exception = e
				print e
			end

			@status = TaskStatus::FINISHED
			@thread = nil
		end
	end

	attr_reader :num

	def initialize(num,&block)
		@num      = num
		@block    = block
		@shutdown = false
		@workers  = []
		@todos    = ConArray.new
		num.times do |i|
			spawn_thead(i)
		end
	end

	def process(*args,&block)
		unless block || @block
			raise ArgumentError,'you must pass a block'
		end

		task = Task.new(self, *args, &(block || @block))
		@todos.add(task)
		task
	end

	def join
		@workers.first.join
		self
	end
private

	def spawn_thead(i)
		thread = Thread.new do
			Thread.current.thread_variable_set(:id, "thread-#{i}")
			while !@shutdown do
				task = @todos.take
				task.execute(thread) if task
				break if task.nil?
			end
		end
		@workers << thread
		thread
	end

end

class DBManager
	@@db = nil
	def self.get_db()
		if @@db == nil then
			@@db = SQLite3::Database.new "test.db"
		end
		return @@db
	end

	def self.get(sql)
		print sql
		db = get_db()
		result = db.execute sql
		result[0]
	end

	def self.exec(sql)
		print sql
		db = get_db()
		db.execute sql
	end
end

class TaskManager
	def self.start()
		date = Time.new.strftime("%Y%m%d")
		result = DBManager.get("select * from status where date=#{date}")
		if result.nil? then
			DBManager.exec("insert into status values(0,0,0,#{date})")
		end
		phase = result[0]
		#phase 0
		#phase 1 company link over
		#phase 2 product link over
		#phase 3 product parse over
		case phase
		when 0 then
			# read company link txt,company.txt
		when 1 then
			# read product link txt,product.txt
		when 2 then
			# continue parse product
		when 3 then
			# parse over
		end
		print result
		# check the task status
		# check company link over
		# if company link fetch not complete then continue fetch
		# if company link fetch over then check fetch product link over
		# if product fetch over
	end

end

class ThreadPool

end

def create_db()
	db = SQLite3::Database.new "test.db"

	db.execute "drop table if exists status;"
	db.execute <<-SQL
		create table status(
			phase int,
			company int,
			product int,
			date int PRIMARY KEY
		);
	SQL
	db.execute "drop table if exists company;"
	db.execute <<-SQL
		create table company (
			mid int PRIMARY KEY,
			name varchar(64),
			addr varchar(128),
			post varchar(16),
			owner varchar(16),
			contact varchar(32)
		);
	SQL
	db.execute "drop table if exists product;"
	db.execute <<-SQL
	    create table product (
			id varchar(64) PRIMARY KEY,
			company_id int,
			name varchar(64),
			type varchar(128),
			sec_no varchar(32),
			std varchar(128),
			range varchar(128),
			device text,
			comment text,
			status varchar(16),
			expire varchar(32)
		);
	SQL

end

def parse_company(id,page)
	name  = page.search("span[id='ContentPlaceHolder1_ContentPlaceHolder1_lbName']")
	address= page.search("span[id='ContentPlaceHolder1_ContentPlaceHolder1_lbAddress']")
	post  = page.search("span[id='ContentPlaceHolder1_ContentPlaceHolder1_lbPost']")
	owner = page.search("span[id='ContentPlaceHolder1_ContentPlaceHolder1_lbCommunicator']")
	contact=page.search("span[id='ContentPlaceHolder1_ContentPlaceHolder1_lbPhone']")

	name = name.nil? ? "" : name.text.strip
	address = address.nil? ? "" : address.text.strip
	post = post.nil? ? "" : post.text.strip
	owner = owner.nil? ? "" : owner.text.strip
	contact = contact.nil? ? "" : contact.text.strip

	sql = <<-EOF
insert or replace into company (mid,name,addr,post,owner,contact) values (#{id},'#{name}','#{address}','#{post}','#{owner}','#{contact}') ;
	EOF
	print sql
	$db.execute sql
	parse_product_list(id,page)

end

def parse_product_list(company_id,page)
	print "--------- 开始解析产品列表 ---------\n"
	trs = page.search("table[@class='ABtableDefault list-table'] tbody tr")
	pcount = trs.length
	1.step(pcount-1,1) do |row|
		pid   = trs[row].search("td[2]").text.strip
		pname = trs[row].search("td[3] a").text.strip
		link  = trs[row].search("td[3]/a/@href").text
		print <<-EOF
--------- #{pid} #{pname} ---------
EOF
		parse_prodcut(company_id,link)
		sleep(1)
		# break
	end
end


def parse_prodcut(company_id,link)
	page = Mechanize.new.get("http://www.aqbz.org/Home/Search/"+link)
	map = {
		:name => "ContentPlaceHolder1_ContentPlaceHolder1_lbcpmc",
		:address => "ContentPlaceHolder1_ContentPlaceHolder1_lbzcdz",
		:type => "ContentPlaceHolder1_ContentPlaceHolder1_lbggxh",
		:sec_no => "ContentPlaceHolder1_ContentPlaceHolder1_lbaqbzbh",
		:expire => "ContentPlaceHolder1_ContentPlaceHolder1_lbyxq",
		:std => "ContentPlaceHolder1_ContentPlaceHolder1_lbbzhyq",
		:range => "ContentPlaceHolder1_ContentPlaceHolder1_lbsyfw",
		:device => "ContentPlaceHolder1_ContentPlaceHolder1_lbzyskybjjglsb",
		:comment => "ContentPlaceHolder1_ContentPlaceHolder1_lbbz",
		:status => "ContentPlaceHolder1_ContentPlaceHolder1_lbabzt"
	}
	vals = {}
	map.each do |k,v|
		val = page.search("span[id='#{v}']")
		val = val.nil? ? "" : val.text.strip
		vals[k]=val
		# print "#{k}=#{val}\n"
	end
	result = /id\=([a-z0-9\-]+)/.match(link)
	id=result[1]
	sql = <<-EOF
insert or replace into product (id,company_id,name,type,sec_no,std,range,device,comment,status,expire) values
('#{id}' , #{company_id} , "#{vals[:name]}" , "#{vals[:type]}" , "#{vals[:sec_no]}" , "#{vals[:std]}" , "#{vals[:range]}" , "#{vals[:device]}" ,"#{vals[:comment]}" , "#{vals[:status]}" , "#{vals[:expire]}");
EOF
	print sql
	$db.execute sql
end

def fetch_company(url)
	begin
		ppage = Mechanize.new.get(url)
		# AB_SHOW_Q.aspx?t1=search&t2=1&mid=41530240
		result = /mid\=(\d+)/.match(url)
		if result then
			parse_company(result[1],ppage)
		end
	rescue => err
		sleep(1)
		print err.to_s
		fetch_company(url)
	end
end

def fetch_page(url)
	page = Mechanize.new.get(url)
	links = page.links_with(href: /AB_SHOW_Q.aspx/)
	links.each do |link|
		print "--------- #{link.text} ---------\n"
		url = "http://www.aqbz.org/Home/Search/"+link.href
		fetch_company(url)
		# break
	end
end

def fetch()
	$db = SQLite3::Database.new "test.db"
	searchurl = "http://www.aqbz.org/Home/Search/QY.aspx?t1=search&t2=1&ab=&qname=%E5%85%AC%E5%8F%B8&cname=&xh=&sheng=all"
	pagenum = Mechanize.new.get(searchurl).search("span[id=ContentPlaceHolder1_ContentPlaceHolder1_lball]").first
	pagenum = pagenum.text.to_i
	baseurl = "http://www.aqbz.org/Home/Search/QY.aspx?t1=search&t2=1&ab=&qname=%E5%85%AC%E5%8F%B8&cname=&xh=&sheng=all"
	1.step(pagenum,1) do |no|
		url = baseurl + "&Page=%s" % [no] + "\n"
		fetch_page(url)
		# break
	end
end

def gen_task()

end

def gen_company_task()

end

def gen_company_task()

end

def main(arg)
	if arg.length == 0 then
		print "usage cmd"
		return
	end

	cmd = arg[0]
	case cmd
	when "createdb" then create_db()
	when "fetch" then fetch()
	when "gen_task" then gen_task()
	when "test" then TaskManager.start()
	when "thread" then
		i = 0
		pool = ThreadPool.new(2)

		pool.process()do
			id = Thread.current.thread_variable_get('id')

				i=i+1
				print "#{id} #{i}\n"
				sleep(1)

		end
		pool.process()do
			id = Thread.current.thread_variable_get('id')

				i=i+1
				print "#{id} #{i}\n"
				sleep(1)

		end
		pool.join
	end
end

main(ARGV)
# main(["fetch"])
