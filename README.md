# 🍽️ Cardápio Digital

API desenvolvida para gerenciamento de pedidos em uma cantina escolar, permitindo que responsáveis realizem pedidos antecipados enquanto a cantina organiza a produção de forma eficiente.

Além da gestão de escolas, cardápios e produtos, o projeto agora também conta com uma **estrutura inicial de autenticação**, com **cadastro e login de usuários**, servindo como base para controle de acesso por perfil dentro do sistema.

---

## 🎯 Objetivo

Digitalizar o processo de agendamento de lanches escolares, centralizando a comunicação entre pais e cantinas em uma plataforma única, escalável e baseada em boas práticas de desenvolvimento.

Nesta fase atual, o foco está em consolidar a base estrutural da API com:

- gerenciamento de escolas
- gerenciamento de cardápios
- gerenciamento de produtos
- relacionamento entre cardápios e produtos
- autenticação de usuários com perfis distintos

---

## ⚙️ Funcionalidades

### 🏫 Cantina

- Cadastro de escolas
- Gerenciamento de cardápios
- Gerenciamento de produtos
- Organização por categorias
- Associação de produtos aos cardápios

### 🍔 Produtos

CRUD completo de produtos:

- Nome
- Descrição
- Preço
- Categoria

### 📋 Cardápios

- Cadastro de múltiplos cardápios por escola
- Associação de produtos em diferentes cardápios
- Estrutura preparada para promoções e cardápios sazonais

### 🔐 Autenticação

- Cadastro de usuários com nome, email, senha e perfil
- Login de usuários com validação de credenciais
- Armazenamento seguro de senha com hash BCrypt
- Validação de email duplicado
- Estrutura inicial de controle de acesso por perfil

### 👨‍👩‍👧 Responsáveis

> **Planejado para as próximas etapas do projeto**

- Cadastro de alunos
- Agendamento de pedidos
- Controle de pedidos por período

---

## 🔐 Autenticação e Controle de Acesso

O sistema possui uma estrutura inicial de autenticação baseada em usuários e perfis de acesso.

### Perfis atuais

Atualmente a autenticação foi pensada para os seguintes perfis:

- **Admin** → responsável pela administração geral do sistema
- **Escola** → responsável por gerenciar cardápios e produtos da escola

> A área dos responsáveis/pais ainda será implementada futuramente.

### Funcionalidades de autenticação implementadas

- Cadastro de usuários
- Login de usuários
- Hash de senha com BCrypt
- Verificação de email já cadastrado
- Validação de senha no login

---

## 📝 Fluxo de Cadastro

O fluxo de cadastro foi pensado para garantir que os dados do usuário sejam validados antes de serem persistidos no banco.

### Etapas do cadastro

1. O usuário acessa a tela de cadastro
2. Preenche **nome, email, senha e perfil**
3. A API valida os dados recebidos
4. O sistema verifica se o email já existe no banco
5. Se o email já existir, o cadastro é bloqueado e um erro é retornado
6. Se o email não existir, a senha é convertida em **hash com BCrypt**
7. O usuário é salvo no banco de dados
8. O cadastro é concluído com sucesso

### Fluxograma de cadastro

Link do fluxograma:  
👉 https://drive.google.com/file/d/1obazlDc6-nbhPAEMvPbMXQlUs1z1-mZn/view?usp=sharing

---

## 🔑 Fluxo de Login

O fluxo de login foi modelado para autenticar usuários já cadastrados no sistema, comparando a senha digitada com o hash salvo no banco de dados.

### Etapas do login

1. O usuário acessa a tela de login
2. Informa **email e senha**
3. A API busca o usuário pelo email
4. O sistema verifica se o usuário existe
5. Se o usuário não existir, retorna erro
6. Se o usuário existir, a senha digitada é comparada com o hash salvo
7. Se a senha estiver incorreta, retorna erro
8. Se a senha estiver correta, o login é validado

### Fluxograma de login

Link do fluxograma:  
👉 https://drive.google.com/file/d/1pCABHWHV6f8E7WtIq3StHCkmBQ5_0t6h/view?usp=sharing

---

## 📌 Observação sobre autenticação

Atualmente o projeto **ainda não utiliza JWT**.  
Nesta etapa, o objetivo foi implementar primeiro a base de autenticação com:

- cadastro de usuários
- login
- hash de senha
- separação por perfil

O próximo passo será evoluir essa estrutura para **autenticação com JWT e autorização por perfil**.

---

## 🧠 Modelagem de Dados

O sistema foi modelado utilizando relacionamentos relacionais com foco em escalabilidade e reutilização de dados.

### 🔗 Relacionamentos principais

```txt
USUARIO
   ├── Perfil (Admin / Escola)

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

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- BCrypt.Net-Next
- DrawSQL
- Git
- GitHub
- Postman

---

## 📁 Estrutura do Projeto

```bash
cardapio_digital/
├── Data/
├── DTOs/
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

- .NET SDK 8+
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

Responsável por cadastrar um novo usuário no sistema.

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

Responsável por autenticar um usuário já cadastrado.

#### Exemplo de body

```json
{
  "email": "escola@email.com",
  "senha": "123456"
}
```

---

## 🏫 Endpoints de escolas

- `GET /escolas`
- `GET /escolas/{id}`
- `POST /escolas`
- `PUT /escolas/{id}`
- `PATCH /escolas/{id}`
- `DELETE /escolas/{id}`

## 📋 Endpoints de cardápios

- `GET /cardapios`
- `GET /cardapios/{id}`
- `POST /cardapios`
- `PUT /cardapios/{id}`
- `PATCH /cardapios/{id}`
- `DELETE /cardapios/{id}`

## 🍔 Endpoints de produtos

- `GET /produtos`
- `GET /produtos/{id}`
- `POST /produtos`
- `PUT /produtos/{id}`
- `PATCH /produtos/{id}`
- `DELETE /produtos/{id}`

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

---

## 🚀 Melhorias futuras

- Autenticação JWT
- Autorização por perfil
- Área dos responsáveis/pais
- Cadastro de alunos
- Agendamento de pedidos
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

---

## 📌 Status

🚧 Em desenvolvimento

---

## 👨‍💻 Autor

Eduardo Vinicius
