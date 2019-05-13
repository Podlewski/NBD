#!/bin/bash
echo "rebooting stopped containers..."
docker-compose up -d
echo "rebooting done"
