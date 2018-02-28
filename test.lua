math.randomseed(os.time())

local rand1 = math.random() * 2
rand1 = rand1 - rand1 % 1
rand1 = (rand1 - 0.5) * 2
print(rand1)
