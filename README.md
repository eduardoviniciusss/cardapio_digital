# 🍽️ Cardápio Digital

API desenvolvida para gerenciamento de pedidos em uma cantina escolar, com cadastro de escolas, cardápios, produtos, usuários, pais e filhos, além de autenticação e autorização por perfil.

Nesta versão, o projeto já incorpora uma estrutura mais completa para o fluxo de negócio da cantina, incluindo cadastro e login de usuários, políticas de acesso por papéis e endpoints protegidos para diferentes perfis.

---

## 🎯 Objetivo

Digitalizar o processo de agendamento e organização de lanches escolares, centralizando a comunicação entre cantina, escolas e responsáveis em uma API única, escalável e preparada para evolução.

Nesta fase atual, o foco está em consolidar:

- gerenciamento de escolas
- gerenciamento de cardápios
- gerenciamento de produtos
- relacionamento entre cardápios e produtos
- cadastro e login de usuários
- autenticação JWT com autorização por perfil
- cadastro de pais e filhos

---

## ⚙️ Funcionalidades

### 🏫 Cantina

- Cadastro e gerenciamento de escolas
- Gerenciamento de cardápios por escola
- Gerenciamento de produtos por escola
- Organização por categorias
- Associação de produtos aos cardápios

### 🍔 Produtos

CRUD completo de produtos:

- Nome
- Preço
- Categoria
- Vinculação à escola

### 📋 Cardápios

- Cadastro de múltiplos cardápios por escola
- Associação de produtos aos cardápios
- Estrutura preparada para promoções e cardápios sazonais

### 🔐 Autenticação e autorização

- Cadastro de usuários com nome, email, senha e perfil
- Login de usuários com validação de credenciais
- Armazenamento seguro de senha com hash BCrypt
- Validação de email duplicado
- Autenticação JWT
- Políticas de autorização para perfis como Administrador, Cantina e Pais

### 👨‍👩‍👧 Responsáveis e filhos

- Cadastro de pais associados a usuários
- Cadastro de filhos vinculados a pais e escolas
- Consulta dos filhos cadastrados por responsável

---

## 🔐 Perfis e controle de acesso

A API já conta com políticas de autorização baseadas em perfis:

- **Administrador** → acesso total à administração do sistema
- **Cantina** → gerenciamento de cardápios, produtos e categorias
- **Pais** → cadastro e consulta de filhos

### Funcionalidades implementadas

- Cadastro de usuários
- Login de usuários
- Hash de senha com BCrypt
- Verificação de email já cadastrado
- Validação de senha no login
- Autenticação JWT
- Respostas de erro para não autenticado e sem permissão

---

## 🧠 Modelagem de Dados

O sistema foi modelado utilizando relacionamentos relacionais com foco em escalabilidade e reutilização de dados.

### 🔗 Relacionamentos principais

```txt
USUARIO
   ├── Perfil (Administrador / Cantina / Pais)

ESCOLA
   └── CARDAPIO
          └── CARDAPIO_PRODUTO
                 └── PRODUTO
                        └── CATEGORIA

PAIS
   └── FILHO
```

### 📌 Regras da modelagem

- Um usuário possui um único perfil de acesso
- Um usuário com perfil Cantina ou Administrador pode estar vinculado a uma escola
- Uma escola pode possuir vários cardápios
- Um cardápio pode possuir vários produtos
- Um produto pode estar em vários cardápios
- Produtos pertencem a categorias
- Um usuário possui um perfil de acesso
- Usuários com perfil **Escola** serão a base para gerenciamento da cantina
- Usuários com perfil **Admin** serão responsáveis pela administração do sistema

### 🗺️ Diagrama da modelagem

👉 https://drawsql.app/teams/eduardovj/diagrams/cardapio-digital

---

## 🛠️ Tecnologias

- .NET 10
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- BCrypt.Net-Next
- JWT Bearer Authentication
- Swagger / OpenAPI
- Git
- GitHub
- Postman

---

## 📁 Estrutura do Projeto

```bash
cardapio_digital/
├── Data/
├── Dtos/
├── Endpoints/
├── Entities/
├── Enums/
├── Migrations/
├── Properties/
├── appsettings.json
├── Program.cs
```

---

## ▶️ Como executar o projeto

### 📋 Pré-requisitos

- .NET SDK 10+
- PostgreSQL
- Git
- VS Code ou Visual Studio
- Postman (opcional para testes)

### 📥 Clonar repositório

```bash
git clone https://github.com/seu-usuario/cardapio_digital.git
cd cardapio_digital
```

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

Adicione também os valores de JWT no `appsettings.json`:

```json
"Jwt": {
  "Issuer": "cardapio-digital",
  "Audience": "cardapio-digital",
  "SecretKey": "sua-chave-secreta-muito-segura"
}
```

### 📦 Restaurar dependências

```bash
dotnet restore
```

### 🧱 Rodar migrations

```bash
dotnet ef database update
```

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

## 🔐 Endpoints de autenticação

### Cadastro de usuário

```http
POST /usuarios
```

#### Exemplo de body

```json
{
  "nome": "Escola ABC",
  "email": "escola@email.com",
  "senha": "123456",
  "perfil": 2
}
```

### Login de usuário

```http
POST /login
```

#### Exemplo de body

```json
{
  "email": "escola@email.com",
  "senha": "123456"
}
```

---

## 🏫 Endpoints de escolas

- `GET /schools`
- `GET /schools/{id}`
- `POST /schools`
- `PUT /schools/{id}`
- `PATCH /schools/{id}`
- `DELETE /schools/{id}`

## 📋 Endpoints de cardápios

- `GET /menus`
- `GET /menus/{id}`
- `POST /menus`
- `PUT /menus/{id}`
- `PATCH /menus/{id}`
- `DELETE /menus/{id}`

## 🍔 Endpoints de produtos

- `GET /products`
- `GET /products/{id}`
- `POST /products`
- `PUT /products/{id}`
- `PATCH /products/{id}`
- `DELETE /products/{id}`

## 🧂 Endpoints de categorias

- `GET /categories`
- `GET /categories/{id}`
- `POST /categories`
- `PUT /categories/{id}`
- `DELETE /categories/{id}`

## 👨‍👩‍👧 Endpoints de pais e filhos

- `POST /parents`
- `POST /children`
- `GET /children`

## 🔗 Endpoints de relacionamento cardápio-produto

- `GET /menus-productos`
- `GET /menus-productos/{cardapioId}/{produtoId}`
- `POST /menus-productos`
- `PUT /menus-productos/{cardapioId}/{produtoId}`
- `PATCH /menus-productos/{cardapioId}/{produtoId}`
- `DELETE /menus-productos/{cardapioId}/{produtoId}`

---

## 🧪 Testes

A API pode ser testada via:

- Swagger
- Postman

### Fluxos já testáveis

#### Autenticação
- Cadastro de usuário
- Login de usuário
- Validação de email duplicado
- Validação de senha incorreta
- Validação de usuário não encontrado

#### Gestão
- CRUD de escolas
- CRUD de cardápios
- CRUD de produtos
- CRUD de categorias
- Cadastro de pais e filhos

---

## 🚀 Melhorias futuras

- Expansão do fluxo de pedidos
- Área dos responsáveis/pais com mais funcionalidades
- Controle financeiro do aluno
- Histórico de pedidos
- Notificações
- Dashboard administrativo
- Upload de imagens dos produtos

---

## ⚠️ Possíveis problemas

- **Erro de conexão** → revisar `ConnectionStrings`
- **Tabelas não existem** → rodar migrations
- **Porta em uso** → alterar `launchSettings.json`
- **Erro no login** → verificar email cadastrado e senha
- **Senha inválida** → verificar se o hash foi salvo corretamente no banco
- **Erro 401/403** → verificar token JWT e permissões do perfil

---

## 📌 Status

🚧 Em desenvolvimento

---

## 👨‍💻 Autor

Eduardo Vinicius
