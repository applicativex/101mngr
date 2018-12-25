docker build -f .\101mngr.Host\Dockerfile -t 101mngr/host .
docker push 101mngr/host

docker build -f .\101mngr.WebApp\Dockerfile -t 101mngr/web-app .
docker push 101mngr/web-app