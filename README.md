# AnimePI

executar sql server e api com as migracoes
docker-compose up --build

executar apenas o sqlserver
docker-compose up sqlserver

execytar sqlserver e api
docker-compose up sqlserver animepi-api --build

aplicar migracoes manuais (se necessario)
docker-compose --profile migration run --rm migration

acessar ferramentas sql
docker-compose --profile tools up mssqltools
docker exec -it animepi-mssqltools /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "Supersenha123!"
