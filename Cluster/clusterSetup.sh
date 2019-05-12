#!/bin/bash

echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating config replica set"
docker exec -it mongocfg1 bash -c "echo 'rs.initiate({_id: \"mongors1conf\",configsvr: true, members: [{ _id : 0, host : \"mongocfg1\" },{ _id : 1, host : \"mongocfg2\" }]})' | mongo"
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating shard1 replica set"
docker exec -it mongors1n1 bash -c "echo 'rs.initiate({_id : \"mongors1\", members: [{ _id : 0, host : \"mongors1n1\" },{ _id : 1, host : \"mongors1n2\" }]})' | mongo"
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ initiating shard2 replica set"
docker exec -it mongors2n1 bash -c "echo 'rs.initiate({_id : \"mongors2\", members: [{ _id : 0, host : \"mongors2n1\" },{ _id : 1, host : \"mongors2n2\" }]})' | mongo"
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ adding shard1"
docker exec -it mongos1 bash -c "echo 'sh.addShard(\"mongors1/mongors1n1\")' | mongo "
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ adding shard2"
docker exec -it mongos1 bash -c "echo 'sh.addShard(\"mongors2/mongors2n1\")' | mongo "
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ creating diabetes db"
docker exec -it mongors1n1 bash -c "echo 'use diabetes' | mongo"
sleep 3
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ enabling sharding on diabetes db"
docker exec -it mongos1 bash -c "echo 'sh.enableSharding(\"diabetes\")' | mongo "
sleep 3
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ creating colletion in diabetes db"
docker exec -it mongors1n1 bash -c "echo 'db.createCollection(\"diabetes.collection\")' | mongo "
sleep 3
echo "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ enabling sharding on diabetes.colletion"
docker exec -it mongos1 bash -c "echo 'sh.shardCollection(\"diabetes.collection\", {\"encounter_id\" : \"hashed\"})' | mongo"
