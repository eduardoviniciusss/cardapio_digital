O **Cardápio Digital** é um sistema webapi para gerenciamento de pedidos de uma cantina escolar.

A ideia é permitir que **pais façam pedidos de lanches para seus filhos de casa**, enquanto a **cantina recebe e prepara os pedidos** de forma organizada.

---

## 🎯 Objetivo

Digitalizar o processo de agendamento de lanches escolares, conectando pais e cantinas em uma plataforma integrada, seguindo boas práticas de desenvolvimento do mercado.

---

## 🚀 Requisitos Funcionais

### 🏠 Lado da Cantina

* Cadastro de Cantina (vinculado a uma instituição de ensino)
* Gestão de Cardápio (CRUD completo)

  * Nome
  * Descrição
  * Preço
  * Categoria

### 👨‍👩‍👧 Lado dos Pais

* Gestão de Alunos (cadastro de filhos e turmas)
* Agendamento de Lanches (mensal/semanal com calendário)
* Edição de Pedidos (respeitando horário limite)

---

## 🧠 Regras de Negócio

* 🔒 **Segurança de Dados**:

  * Pais só acessam dados dos próprios filhos
  * Cantina só visualiza pedidos vinculados a ela

* ⏰ **Horário de Corte**:

  * Definição de horário limite (ex: 21:00 do dia anterior)
  * Após isso, não é possível alterar pedidos

* 📊 **Relatório de Produção**:

  * Sistema soma todos os pedidos
  * Gera lista de preparo para a cantina

---

## 🛠️ Tecnologias utilizadas

* .NET Core
* Entity Framework
* Git

---

## 📁 Estrutura atual do projeto

* Criação do projeto com .NET Core
* Endpoint de teste `/healthy`
* Configuração do `.gitignore` (bin/ e obj/)
* Integração com Entity Framework

---

## ▶️ Como rodar o projeto

1. Clone o repositório:

git clone [https://github.com/seu-usuario/cardapio_digital.git](https://github.com/seu-usuario/cardapio_digital.git)

2. Acesse a pasta:

cd cardapio_digital

3. Execute:

dotnet run

## 📌 Status do projeto

🚧 Em desenvolvimento

---

## 👨‍💻 Autor

Eduardo Vinicius