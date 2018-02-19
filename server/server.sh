#!/bin/sh
[[ $# -lt 1 ]] && echo "useage sh server.sh start|stop|reload" && exit;
cmd=$1

case $cmd in
    start)
       openresty -p `pwd`/ -c "./conf/nginx.conf"
    ;;
    stop)
        openresty -p `pwd`/ -s stop
    ;;
    reload)
        openresty -p `pwd`/ -s reload
    ;;
    *)
        echo "$cmd is not supported!"
    ;;
esac
