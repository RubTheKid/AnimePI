# AnimePI

Uma API completa para gerenciamento de animes e favoritos de usuários, desenvolvida com .NET 8, Entity Framework Core, SQL Server e integração com a API externa Jikan (MyAnimeList).

## Arquitetura

#### Arquitetura de camadas

* AnimePI.Domain: Entidades de domínio, interfaces de repositórios e regras de negócio

* AnimePI.Application: Serviços, CQRS (Commands/Queries), DTOs e interfaces de aplicação

* AnimePI.Infra: Implementação de repositórios, Entity Framework, migrações e mapeamentos

* AnimePI.Api: Controllers, configuração de DI e endpoints REST

* AnimePI.Tests: Testes unitários e de integração


#### CQRS

O projeto implementa o padrão CQRS com MediatR para separar operações de leitura (Queries) e escrita (Commands):

* Commands: Operações que modificam o estado (Create, Update, Delete)

* Queries: Operações de consulta (Get, List, Search)

* Handlers: Processadores que executam a lógica de negócio

### Banco de dados

O projeto utiliza SQL Server rodando em container Docker, com as seguintes tabelas:
``` 
	Users (1) ←→ (0..1) Favorites
	├── Id (PK, Guid)
	├── Name (varchar)
	├── Surname (varchar)
	├── Email (varchar)
	└── DateCreated, DateUpdated, IsDeleted

	Favorites (1) ←→ (*) AnimeFavorites (JSON)
	├── UserId (PK, FK → Users.Id)
	├── Animes (nvarchar(max), JSON serializado)
	└── DateCreated, DateUpdated, IsDeleted

	Animes (Dados da API Jikan)
	├── Id (PK, Guid)
	├── MalId (int, Unique - ID do MyAnimeList)
	├── Title, TitleEnglish, TitleJapanese
	├── Synopsis, ImageUrl, TrailerUrl
	├── Score, Rank, Episodes, Status, Type
	├── Genres (JSON), Studios (JSON)
	└── DateCreated, DateUpdated, IsDeleted
``` 

## Integração com API Jikan

A aplicação consome dados da Jikan API (https://api.jikan.moe/v4/) que fornece informações detalhadas sobre animes:

* Streaming de Dados: Os dados são buscados da API externa e persistidos no banco local

* Cache Local: Permite consultas rápidas sem depender da API externa

* Paginação: Suporte para buscar animes em lotes (25 por página)

* Rate Limiting: Respeitando o limite de 3 requests/segundo da Jikan API

### Endpoints de integração
```
POST /api/anime/fetch/top?page=1&limit=25 - Busca top animes (1-25, 26-50, etc.)
POST /api/anime/fetch/top/multiple-pages?startPage=1&endPage=4 - Busca múltiplas páginas
POST /api/anime/fetch/season/spring/2024 - Busca animes por temporada
POST /api/anime/fetch/{malId} - Busca anime específico pelo ID
```

## Docker e containerização

#### Containers
* SQL Server: Container com SQL Server 2019 Express

* API: Container com a aplicação .NET 8

* Volumes: Persistência de dados do SQL Server

# Execução

#### Pré-requisitos
* Docker Desktop
* .Net 8 SDK
* SQL Server Management Studio (ou similares) para acesso ao banco (opcional)

### Executar SQL Server e Api com migrações
```
docker-compose up --build
```

### Acessar a aplicação

* API
```
URL: http://localhost:8080
Swagger: http://localhost:8080/swagger
Porta: 8080 (mapeada do container)
```

* Banco de Dados
```
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: Supersenha123!
Database: AnimePI
```

## Funcionalidades

#### Gerenciamento de Usuários
 * Criar, listar, atualizar e deletar usuários
 * Validação de email e nome
 * Soft delete com controle de data

#### Sistema de Favoritos
 * Adicionar/remover animes dos favoritos
 * Toggle de favoritos (adiciona se não existe, remove se existe)
 * Listar favoritos do usuário
 * Verificar se anime é favorito

 #### Catálogo de Animes
 * Buscar animes locais por título, temporada, ranking
 * Integração com Jikan API para dados atualizados
 * Cache local para performance
 * Análise de gêneros mais populares

 ## Tecnologias utilizadas
 * .NET 8 - Framework principal
 * EF Core - ORM
 * SQL Server - Banco de dados Relacional
 * Docker - Containerização
 * Swagger - Documentação da API
 * HttpClient - Consumo de APIs Externas