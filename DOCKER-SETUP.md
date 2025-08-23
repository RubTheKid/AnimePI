# Configuração Docker para AnimePI

## Visão Geral
Este projeto usa Docker Compose para gerenciar o SQL Server e a API. Por padrão, apenas o SQL Server é executado para permitir a aplicação de migrações.

## Comandos Docker Compose

### 1. Executar apenas o SQL Server (Recomendado para desenvolvimento)
```bash
docker-compose up sqlserver
```
Este comando executa apenas o SQL Server na porta 1433.

### 2. Executar SQL Server em background
```bash
docker-compose up -d sqlserver
```

### 3. Executar tudo (SQL Server + API)
```bash
docker-compose --profile api up
```

### 4. Executar com ferramentas SQL Server
```bash
docker-compose --profile tools up
```

### 5. Parar todos os serviços
```bash
docker-compose down
```

### 6. Parar e remover volumes
```bash
docker-compose down -v
```

## Configurações de Conexão

### Desenvolvimento Local
- **Servidor**: localhost,1433
- **Database**: AnimePI
- **Usuário**: sa
- **Senha**: SqlServer2022!
- **TrustServerCertificate**: true

### Docker (API)
- **Servidor**: sqlserver
- **Database**: AnimePI
- **Usuário**: sa
- **Senha**: SqlServer2022!
- **TrustServerCertificate**: true

## Aplicando Migrações

1. **Inicie o SQL Server**:
   ```bash
   docker-compose up -d sqlserver
   ```

2. **Aguarde o healthcheck** (verifique com `docker-compose ps`)

3. **Execute as migrações**:
   ```bash
   dotnet ef database update --project AnimePI.Infra --startup-project AnimePI.Api
   ```

## Verificando o Status

```bash
# Ver status dos containers
docker-compose ps

# Ver logs do SQL Server
docker-compose logs sqlserver

# Conectar ao SQL Server
docker exec -it animepi-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P SqlServer2022!
```

## Troubleshooting

### SQL Server não inicia
- Verifique se a porta 1433 está livre
- Verifique os logs: `docker-compose logs sqlserver`

### Problemas de conexão
- Aguarde o healthcheck completar
- Verifique se o container está rodando: `docker-compose ps`
- Teste a conexão: `docker exec -it animepi-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P SqlServer2022! -Q "SELECT 1"`

### Reset completo
```bash
docker-compose down -v
docker-compose up -d sqlserver
```
