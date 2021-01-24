docker-compose  -f docker-compose.yml -f docker-compose.override.prod.yml --no-ansi config  
docker-compose  -f docker-compose.yml -f docker-compose.override.prod.yml --no-ansi build --force-rm
docker-compose  -f docker-compose.yml -f docker-compose.override.prod.yml --no-ansi up -d --no-build --force-recreate 

