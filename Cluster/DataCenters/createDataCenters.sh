#!/bin/bash

#docker-machine create --driver virtualbox center1
#docker-machine create --driver virtualbox center2

echo "~~~~~~~~~~~~~~~~~~switching to center1"
eval $(docker-machine env center1)

docker-machine ls

echo "~~~~~~~~~~~~~~~~~~initializing swarm on center1"
docker swarm init --advertise-addr $(docker-machine ip center1):2377

echo "~~~~~~~~~~~~~~~~~~adding center2 to swarm"
docker-machine ssh center2 "$(docker swarm join-token worker | grep -e '--token')"

docker node update --label-add mongo.datacenter=1 $(docker node ls -qf name=center1)
docker node update --label-add mongo.datacenter=1 $(docker node ls -qf name=center1)
