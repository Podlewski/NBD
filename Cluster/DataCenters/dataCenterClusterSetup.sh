#!/bin/bash

initConfigRS='rs.initiate({_id: "mongors1conf",configsvr: true, members: [{ _id : 0, host : "mongocfg1" },{ _id : 1, host : "mongocfg2" }]})'
initRS1='rs.initiate({_id : "mongors1", members: [{ _id : 0, host : "mongors1n1" },{ _id : 1, host : "mongors1n2" },{ _id : 2, host : "mongors1n3" }]})'
initRS2='rs.initiate({_id : "mongors2", members: [{ _id : 0, host : "mongors2n1" },{ _id : 1, host : "mongors2n2" },{ _id : 2, host : "mongors2n3" }]})'
addS1='sh.addShard("mongors1/mongors1n1")'
addS2='sh.addShard("mongors2/mongors2n1")'
enableShrd='sh.enableSharding("diabetes")'
createCol='db.createCollection("diabetes.collection")'
shrdCol='sh.shardCollection("diabetes.collection", {"encounter_id" : "hashed"})'

switchCenter(){
	eval $(docker-machine env $1)
}


mongo(){
	command="docker exec -i $1 mongo --eval '$2'"
	eval "$command"
}

getName(){
	command=`docker ps | grep $1 | awk '{print \$1;}'`
	echo $command
}

switchCenter center1

echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating config replica set"
mongo $(getName mongocfg1) "$initConfigRS"
sleep 10
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating shard1 replica set"
mongo $(getName mongors1n1) "$initRS1"
sleep 3

switchCenter center2

echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating shard2 replica set"
mongo $(getName mongors2n1) "$initRS2"
sleep 3
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ adding shard1"
mongo $(getName mongos2) "$addS1"
sleep 2
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ adding shard2"
mongo $(getName mongos2) "$addS2"
sleep 2

switchCenter center1

echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ creating diabetes db"
docker exec -i $(getName mongors1n1) bash -c "echo 'use diabetes' | mongo"
sleep 2
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ enabling sharding on diabetes db"
mongo $(getName mongos1) "$enableShrd"
sleep 2
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ creating colletion in diabetes db"
mongo $(getName mongors1n1) "$createCol"
sleep 2
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ enabling sharding on diabetes.colletion"
mongo $(getName mongos1) "$shrdCol"
