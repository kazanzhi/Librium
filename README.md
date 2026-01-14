## Librium – Clean Architecture Library Management System (.NET 9)

Librium is a modular, extensible Library Management System built with Clean Architecture, Domain-Driven Design (DDD) principles, and .NET 9.
The project demonstrates production-grade backend engineering: rich domain models, domain invariants, strict separation of layers, repository abstraction, token-based authentication, and a full test suite for domain logic.

## Core Features
### 📚 Global Book Catalog
* Create, update, delete, and retrieve books
* Manage book categories
* Many-to-many relationship: Book ↔ Category
* All validations and invariants enforced inside the domain
* Domain entities are created only via factory methods
* Server-side filtering via query parameters (title/author)

### 👤 User Personal Library
* Each user has a personal library
* Users can:
   * add books to their library
   * remove books from their library
* Implemented as a DDD aggregate:
   * UserLibrary (aggregate root)
   * LibraryBook (internal entity)
* Book does not depend on users
* Deleting a book from the global catalog removes it from all user libraries
* Removing a book from a user library does not affect the global catalog

### 🔐 Authentication & Authorization
* JWT-based authentication
* Role-based authorization:
   * User
   * Admin
* Clear responsibility split:
   * Application layer — use cases (Login, Register)
   * Identity layer (Infrastructure) — ASP.NET Identity, UserManager, RoleManager
* Login returns JWT
* Registration does not return a token

### Domain-Driven Design
* Rich domain models (no anemic entities)
* Domain layer has no dependency on:
   * ASP.NET
   * Identity
   * EF Core
* All business rules enforced in domain entities
* Constructors are private; creation via factory methods only

### Testing
* Unit tests for:
   * Domain layer
   * Application layer
* Tools:
   * xUnit
   * FluentAssertions
   * Moq
* Tests focus on behavior, not infrastructure
* Explicit error handling via ValueOrResult<T>

### Core Domain Models
* Book
* Root entity of the global catalog
* Fields:
   * Title
   * Author
   * Content
   * PublishedYear
   * Categories
* Owns its invariants
* Completely independent from users

### Category
* Represents a book category
* Many-to-many relationship with Book

### UserLibrary (Aggregate Root)
* Represents a user’s personal library
* Contains a collection of LibraryBook
* Responsible for:
   * AddBook
   * RemoveBook

### LibraryBook
* Internal entity inside UserLibrary
* Stores only BookId
* Not an aggregate root

## ValueOrResult Pattern
The project uses an explicit result type instead of exceptions or nulls:
```
ValueOrResult
ValueOrResult<T>
```

Benefits:
* Predictable error handling
* Explicit control flow
* Clean use-case orchestration
* No hidden exceptions
  
## Database & Persistence
### EF Core Configurations

All domain entities have configurations in `Librium.Persistence.Configurations.`

Domain constructors are private `(private Book() { })` to enforce factory creation.

### Migrations

Generated in the Persistence project:

cd Librium.Persistence
```
dotnet ef migrations add Init -p Librium.Persistence -s Librium.Presentation
```
```
dotnet ef database update -p Librium.Persistence -s Librium.Presentation
```
## Authentication
JWTBearer setup
```
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidateAudience = true,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
ValidIssuer = config["JwtOptions:Issuer"],
ValidAudience = config["JwtOptions:Audience"],
IssuerSigningKey = new SymmetricSecurityKey(
Encoding.UTF8.GetBytes(config["JwtOptions:Key"]!)
)
};
```
## Roles:
* User
* Admin

Protected endpoints use:
```
[Authorize(Roles = UserRoles.User)]
[Authorize(Roles = UserRoles.Admin)]
```
## Docker Support
Librium API is packaged as a Docker image for cloud deployment.

Dockerfile location:
```
backend/Librium.Presentation/Dockerfile
```
Build locally:
```
docker build -t librium-api ./backend -f ./backend/Librium.Presentation/Dockerfile
```
## CI/CD Pipeline (GitHub Actions + Docker + Azure)

* GitHub Actions
* CI:
   * build
   * test
   * docker build
   * Push to Azure Container Registry (ACR)
* CD:
   * push image
   * deploy to Azure Web App for Containers

## How to Run Tests
Run all tests:
```
dotnet test
```
Run only Book tests:
```
dotnet test --filter FullyQualifiedName~BookTests
```
## How to Run (5 Minutes)
### 1. Clone the repo
```
git clone https://github.com/yourname/Librium.git
cd Librium/backend
```
### 2. Configure SQL Server
Create database and update appsettings.json

### 3. Run migrations
```
cd Librium.Persistence
dotnet ef database update -p Librium.Persistence -s Librium.Presentation
```
### 4. Run the app
```
dotnet run --project Librium.Presentation
```
### 5. Try it out!
Open `https://localhost:7213/swagger` and play with tasks



