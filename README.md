# 🍽️ Digital Menu

API developed for managing orders in a school canteen, including registration of schools, menus, products, users, parents, and children, as well as authentication and role-based authorization.

In this version, the project already incorporates a more complete structure for the canteen business workflow, including user registration and login, role-based access policies, and protected endpoints for different user profiles.

---

## 🎯 Objective

Digitize the process of scheduling and organizing school snacks, centralizing communication between the canteen, schools, and parents/guardians through a single, scalable API prepared for future development.

At this stage, the main focus is consolidating:

* School management
* Menu management
* Product management
* Relationship between menus and products
* User registration and login
* JWT authentication with role-based authorization
* Parent and child registration

---

## ⚙️ Features

### 🏫 Canteen

* School registration and management
* Menu management by school
* Product management by school
* Organization by categories
* Association of products with menus

### 🍔 Products

Complete CRUD for products:

* Name
* Price
* Category
* School association

### 📋 Menus

* Registration of multiple menus per school
* Association of products with menus
* Structure prepared for promotions and seasonal menus

### 🔐 Authentication and Authorization

* User registration with name, email, password, and role
* User login with credential validation
* Secure password storage using BCrypt hashing
* Duplicate email validation
* JWT authentication
* Authorization policies for roles such as Administrator, Canteen, and Parent

### 👨‍👩‍👧 Parents and Children

* Registration of parents associated with users
* Registration of children linked to parents and schools
* Retrieval of children registered by a parent/guardian

---

## 🔐 Roles and Access Control

The API already includes role-based authorization policies:

* **Administrator** → full access to system administration
* **Canteen** → management of menus, products, and categories
* **Parent** → registration and retrieval of children

### Implemented Features

* User registration
* User login
* Password hashing with BCrypt
* Verification of already registered emails
* Password validation during login
* JWT authentication
* Error responses for unauthenticated and unauthorized requests

---

## 🧠 Data Modeling

The system was modeled using relational relationships, with a focus on scalability and data reuse.

### 🔗 Main Relationships

```txt
USER
   ├── Role (Administrator / Canteen / Parent)

SCHOOL
   └── MENU
          └── MENU_PRODUCT
                 └── PRODUCT
                        └── CATEGORY

PARENT
   └── CHILD
```

### 📌 Modeling Rules

* A user has a single access role
* A user with the Canteen or Administrator role can be associated with a school
* A school can have multiple menus
* A menu can contain multiple products
* A product can belong to multiple menus
* Products belong to categories
* A user has a single access role
* Users with the **School** role will serve as the basis for canteen management
* Users with the **Admin** role will be responsible for system administration

### 🗺️ Data Model Diagram

👉 https://drawsql.app/teams/eduardovj/diagrams/cardapio-digital

---

## 🛠️ Technologies

* .NET 10
* ASP.NET Core Minimal API
* Entity Framework Core
* PostgreSQL
* BCrypt.Net-Next
* JWT Bearer Authentication
* Swagger / OpenAPI
* Git
* GitHub
* Postman

---

## 📁 Project Structure

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

## ▶️ How to Run the Project

### 📋 Prerequisites

* .NET SDK 10+
* PostgreSQL
* Git
* VS Code or Visual Studio
* Postman (optional for testing)

### 📥 Clone the Repository

```bash
git clone https://github.com/your-username/cardapio_digital.git
cd cardapio_digital
```

### ⚙️ Configure the Database

Create the database:

```text
cantina_digital
```

Configure it in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=cantina_digital;Username=your_username;Password=your_password"
}
```

Also add the JWT configuration to `appsettings.json`:

```json
"Jwt": {
  "Issuer": "cardapio-digital",
  "Audience": "cardapio-digital",
  "SecretKey": "your-very-secure-secret-key"
}
```

### 📦 Restore Dependencies

```bash
dotnet restore
```

### 🧱 Run Migrations

```bash
dotnet ef database update
```

### ▶️ Run the Application

```bash
dotnet run
```

---

## 🌐 Endpoints

Base URL:

```text
http://localhost:5000
```

## 🔐 Authentication Endpoints

### User Registration

```http
POST /usuarios
```

#### Example Body

```json
{
  "nome": "School ABC",
  "email": "escola@email.com",
  "senha": "123456",
  "perfil": 2
}
```

### User Login

```http
POST /login
```

#### Example Body

```json
{
  "email": "escola@email.com",
  "senha": "123456"
}
```

---

## 🏫 School Endpoints

* `GET /schools`
* `GET /schools/{id}`
* `POST /schools`
* `PUT /schools/{id}`
* `PATCH /schools/{id}`
* `DELETE /schools/{id}`

## 📋 Menu Endpoints

* `GET /menus`
* `GET /menus/{id}`
* `POST /menus`
* `PUT /menus/{id}`
* `PATCH /menus/{id}`
* `DELETE /menus/{id}`

## 🍔 Product Endpoints

* `GET /products`
* `GET /products/{id}`
* `POST /products`
* `PUT /products/{id}`
* `PATCH /products/{id}`
* `DELETE /products/{id}`

## 🧂 Category Endpoints

* `GET /categories`
* `GET /categories/{id}`
* `POST /categories`
* `PUT /categories/{id}`
* `DELETE /categories/{id}`

## 👨‍👩‍👧 Parent and Child Endpoints

* `POST /parents`
* `POST /children`
* `GET /children`

## 🔗 Menu-Product Relationship Endpoints

* `GET /menu-products`
* `GET /menu-products/{cardapioId}/{produtoId}`
* `POST /menu-products`
* `PUT /menu-products/{cardapioId}/{produtoId}`
* `PATCH /menu-products/{cardapioId}/{produtoId}`
* `DELETE /menu-products/{cardapioId}/{produtoId}`

---

## 🧪 Testing

The API can be tested using:

* Swagger
* Postman

### Testable Workflows

#### Authentication

* User registration
* User login
* Duplicate email validation
* Incorrect password validation
* User not found validation

#### Management

* School CRUD
* Menu CRUD
* Product CRUD
* Category CRUD
* Parent and child registration

---

## 🚀 Future Improvements

* Expansion of the order management workflow
* Parent/guardian area with additional features
* Student financial management
* Order history
* Notifications
* Administrative dashboard
* Product image uploads

---

## ⚠️ Possible Issues

* **Connection error** → check `ConnectionStrings`
* **Tables do not exist** → run migrations
* **Port already in use** → change `launchSettings.json`
* **Login error** → verify that the email is registered and the password is correct
* **Invalid password** → verify that the password hash was correctly saved in the database
* **401/403 error** → check the JWT token and the user's role permissions

---

## 📌 Status

🚧 **In Development**

---

## 👨‍💻 Author

Eduardo Vinicius
