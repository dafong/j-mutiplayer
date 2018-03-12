#!/bin/sh
rsync -rv --filter='- logs/' --filter='- *_temp/' -e 'ssh -p 61003' server/ root@qq:/data/apps/step

ssh root@qq -p 61003 'cd /data/apps/step;sh server.sh reload;'
