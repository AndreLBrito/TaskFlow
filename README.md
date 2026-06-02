<img width="1313" height="457" alt="TaskFlow2" src="https://github.com/user-attachments/assets/d5609e06-6d1d-4253-b77a-56baf644f0ba" />
<img width="1315" height="409" alt="TaskFlow3" src="https://github.com/user-attachments/assets/0ee6f36d-7d4c-4a33-be88-e761ab870d44" />
<img width="1331" height="584" alt="TaskFlow4" src="https://github.com/user-attachments/assets/a32cec0e-4c24-4da6-aeb5-d2701cf93e12" />
<img width="1267" height="397" alt="TaskFlow1" src="https://github.com/user-attachments/assets/ff0998d0-0760-4088-a3ec-b97c3c7bb216" />
# TaskFlow

![Build](https://github.com/AndreLBrito/TaskFlow/actions/workflows/dotnet.yml/badge.svg)

TaskFlow é uma aplicação web de gerenciamento de tarefas inspirada em ferramentas Kanban como Trello e Jira. O projeto foi desenvolvido com foco em boas práticas de arquitetura, separação de responsabilidades, testes automatizados e tecnologias modernas do ecossistema .NET.

## Tecnologias Utilizadas

### Backend

* ASP.NET Core MVC
* .NET 10
* Entity Framework Core
* PostgreSQL
* MediatR
* FluentValidation
* Mapster
* Serilog

### Arquitetura

* Clean Architecture
* CQRS com MediatR
* Repository Pattern
* Domain-Driven Design (DDD) simplificado

### Testes

* xUnit
* Moq
* FluentAssertions

### Infraestrutura

* Docker
* Docker Compose

## Funcionalidades

### Dashboard

* Total de Workspaces
* Total de Quadros
* Total de Tarefas
* Total de Tarefas Concluídas

### Workspaces

* Criar Workspace
* Visualizar Workspaces
* Editar Workspace
* Excluir Workspace

### Quadros

* Criar Quadros
* Visualizar Quadros
* Editar Quadros
* Excluir Quadros

### Colunas

Ao criar um quadro, o sistema cria automaticamente as colunas padrão:

* A Fazer
* Em Andamento
* Concluído

### Tarefas

* Criar Tarefas
* Visualizar Detalhes
* Editar Tarefas
* Excluir Tarefas
* Mover entre Colunas

### Logs

* Registro de erros utilizando Serilog
* Logs em arquivo
* Logs no console

## Estrutura do Projeto

```text
src
├── TaskFlow.Domain
├── TaskFlow.Application
├── TaskFlow.Infrastructure
└── TaskFlow.Web

tests
└── TaskFlow.UnitTests
```

### TaskFlow.Domain

Contém as entidades e regras de negócio.

### TaskFlow.Application

Contém:

* Commands
* Queries
* Validators
* Handlers
* Interfaces

### TaskFlow.Infrastructure

Contém:

* Entity Framework Core
* Repositórios
* Configurações do banco
* Migrations

### TaskFlow.Web

Contém:

* Controllers
* Views
* ViewModels
* Filtros
* Configurações MVC

## Como Executar o Projeto

### Pré-requisitos

* .NET 9 SDK
* Docker Desktop
* PostgreSQL (opcional se utilizar Docker)

### Clonar o Repositório

```bash
git clone https://github.com/seu-usuario/taskflow.git

cd taskflow
```

### Subir o Banco de Dados

```bash
docker compose up -d
```

### Aplicar as Migrations

```bash
dotnet ef database update \
    --project src/TaskFlow.Infrastructure \
    --startup-project src/TaskFlow.Web
```

### Executar a Aplicação

```bash
dotnet run --project src/TaskFlow.Web
```

A aplicação estará disponível em:

```text
https://localhost:5001
```

ou

```text
http://localhost:5000
```

## Executando os Testes

```bash
dotnet test
```

## Conceitos Demonstrados

Este projeto demonstra experiência prática com:

* ASP.NET Core MVC
* Clean Architecture
* CQRS
* MediatR
* Entity Framework Core
* PostgreSQL
* Docker
* FluentValidation
* Mapster
* Serilog
* Testes Unitários
* Repository Pattern
* Boas práticas de desenvolvimento em C#

## Objetivo

O objetivo deste projeto é demonstrar conhecimentos em desenvolvimento backend com .NET, arquitetura de software, modelagem de domínio e construção de aplicações web escaláveis e organizadas.
