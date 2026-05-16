# 🍽️ Cardápio Digital

API desenvolvida para gerenciamento de pedidos em uma cantina escolar, permitindo que responsáveis realizem pedidos antecipados enquanto a cantina organiza a produção de forma eficiente.

---

## 🎯 Objetivo

Digitalizar o processo de agendamento de lanches escolares, centralizando a comunicação entre pais e cantinas em uma plataforma única, escalável e baseada em boas práticas de desenvolvimento.

---

## ⚙️ Funcionalidades

### 🏫 Cantina

* Cadastro de escolas
* Gerenciamento de cardápios
* Gerenciamento de produtos
* Organização por categorias
* Associação de produtos aos cardápios

### 🍔 Produtos

CRUD completo de produtos:

* Nome
* Descrição
* Preço
* Categoria

### 📋 Cardápios

* Cadastro de múltiplos cardápios por escola
* Associação de produtos em diferentes cardápios
* Estrutura preparada para promoções e cardápios sazonais

### 👨‍👩‍👧 Responsáveis

* Cadastro de alunos
* Agendamento de pedidos
* Controle de pedidos por período

---

## 🧠 Modelagem de Dados

O sistema foi modelado utilizando relacionamentos relacionais com foco em escalabilidade e reutilização de dados.

### 🔗 Relacionamentos principais

```txt
ESCOLA
   |
CARDAPIO
   |
CARDAPIO_PRODUTO
   |
PRODUTO
   |
CATEGORIA
```

### 📌 Regras da modelagem

* Uma escola pode possuir vários cardápios
* Um cardápio pode possuir vários produtos
* Um produto pode estar em vários cardápios
* Produtos pertencem a categorias

### 🗺️ Diagrama da modelagem

👉 https://drawsql.app/teams/eduardovj/diagrams/cardapio-digital

---

## 🛠️ Tecnologias

* .NET 8
* ASP.NET Core Minimal API
* Entity Framework Core
* PostgreSQL
* DrawSQL
* Git
* GitHub

---

## 📁 Estrutura do Projeto

```bash
/src
  /Controllers
  /Entities
  /DTOs
  /Data
  /Migrations
```

---

## ▶️ Como executar o projeto

### 📋 Pré-requisitos

* .NET SDK 8+
* PostgreSQL
* Git
* VS Code ou Visual Studio

---

### 📥 Clonar repositório

```bash
git clone https://github.com/seu-usuario/cardapio_digital.git
cd cardapio_digital
```

---

### ⚙️ Configurar banco de dados

Crie o banco:

```bash
cantina_digital
```

Configure no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=cantina_digital;Username=seu_usuario;Password=sua_senha"
}
```

---

### 🧱 Rodar migrations

```bash
dotnet ef database update
```

---

### ▶️ Executar aplicação

```bash
dotnet run
```

---

## 🌐 Endpoints

Base URL:

```txt
http://localhost:5000
```

Exemplo:

```http
GET /healthy
```

---

## 🧪 Testes

A API pode ser testada via:

* Swagger
* Postman

---

## 🚀 Melhorias futuras

* Autenticação JWT
* Controle financeiro do aluno
* Histórico de pedidos
* Notificações
* Dashboard administrativo
* Upload de imagens dos produtos

---

## ⚠️ Possíveis problemas

* Erro de conexão → revisar connection string
* Tabelas não existem → rodar migrations
* Porta em uso → alterar `launchSettings.json`

---

## 📌 Status

🚧 Em desenvolvimento

---

## 👨‍💻 Autor

Eduardo Vinicius

