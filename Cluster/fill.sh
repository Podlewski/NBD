#!/bin/bash 

mongoimport --jsonArray --host $(docker-machine ip default):27019 --db diabetes --collection collection --file data_10.json
docker exec -it mongos1 bash -c "echo -e 'db.createUser({user: \"admin\", pwd: \"abc123\!\", roles: [{ role: \"root\", db: \"admin\" }]})' | mongo"
