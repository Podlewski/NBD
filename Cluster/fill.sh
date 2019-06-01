#!/bin/bash 

if [ $# -ne 1 ]
then
	echo -e 'Ilegal number of arguments.\n\nUsage:\n fill10.sh {host}\n\nMake sure to be on cluser1 machine!'
	exit 1
fi

mongoimport --jsonArray --host $1:27019 --db diabetes --collection collection --file ../DatabaseScripts/data.json
docker exec -i $(docker ps | grep mongos1 | awk '{print $1;}') bash -c "echo -e 'db.createUser({user: \"admin\", pwd: \"abc123\!\", roles: [{ role: \"root\", db: \"admin\" }]})' | mongo"
