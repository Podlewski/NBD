#!/bin/bash 

mongoimport --jsonArray --host 192.168.99.101:27019 --db diabetes --collection collection --file data_10.json
docker exec -it mongos1 bash -c "echo -e 'db.createUser({user: \"admi\", pwd: \"abc123\!\", roles: [{ role: \"root\", db: \"admin\" }]})' | mongo"
