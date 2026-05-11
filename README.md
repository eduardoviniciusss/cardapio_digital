# 🍽️ Cardápio Digital

API desenvolvida para gerenciamento de pedidos em uma cantina escolar, permitindo que responsáveis realizem pedidos antecipados enquanto a cantina organiza a produção de forma eficiente.

---

## 🎯 Objetivo

Digitalizar o processo de agendamento de lanches escolares, centralizando a comunicação entre pais e cantinas em uma plataforma única, escalável e baseada em boas práticas de desenvolvimento.

---

## ⚙️ Funcionalidades

### 🏫 Cantina

* Cadastro de cantina vinculada a uma instituição
* Gerenciamento de cardápio (CRUD)

  * Nome
  * Descrição
  * Preço
  * Categoria

### 👨‍👩‍👧 Responsáveis

* Cadastro de alunos (filhos e turmas)
* Agendamento de pedidos (semanal/mensal)
* Edição de pedidos com controle de prazo

---

## 🛠️ Tecnologias

* .NET 8
* Entity Framework Core
* PostgreSQL
* Git

---

## 📁 Estrutura do Projeto

```bash
/src
  /Controllers
  /Entities
  /DTOs
  /Data
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

### 🌐 Endpoints

Base URL:

```
http://localhost:5000
```

Exemplo:

```
GET /healthy
```

---

## 🧪 Testes

A API pode ser testada via:

* Swagger (quando habilitado)
* Postman

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

