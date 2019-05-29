#!/bin/bash

if [ -z $(docker-machine ls -q --filter name=center2) ]
then 
	docker-machine create --driver virtualbox center1
	docker-machine create --driver virtualbox center2

	echo "~~~~~~~~~~~~~~~~~~switching to center1"
	eval $(docker-machine env center1)

	echo "~~~~~~~~~~~~~~~~~~initializing swarm on center1"
	docker swarm init --advertise-addr $(docker-machine ip center1):2377

	echo "~~~~~~~~~~~~~~~~~~adding center2 to swarm"
	docker-machine ssh center2 "$(docker swarm join-token worker | grep -e '--token')"

	echo "~~~~~~~~~~~~~~~~~~adding labels to machines"
	docker node update --label-add mongo.datacenter=1 $(docker node ls -qf name=center1)
	docker node update --label-add mongo.datacenter=2 $(docker node ls -qf name=center2)
else
	docker-machine start center1
	docker-machine start center2
fi

docker-machine ls
