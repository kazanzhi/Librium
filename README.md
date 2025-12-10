## Librium – Clean Architecture Library Management System (.NET 9)

Librium is a modular, extensible Library Management System built with Clean Architecture, Domain-Driven Design (DDD) principles, and .NET 9.
The project demonstrates production-grade backend engineering: rich domain models, domain invariants, strict separation of layers, repository abstraction, token-based authentication, and a full test suite for domain logic.

## Core Features
### Books & Categories

* Create, update, retrieve books

* Create and manage book categories

* Domain-level validation and invariants

* Many-to-one relationship (Books → Category)

### User Personal Library

* Users can add/remove books to their personal library

* Relationship modeled through the UserBook entity

* Business rules enforced inside domain models (AppUser.AddBook, RemoveBook, UserBook.Create)

### Authentication & Authorization

* JWT-based authentication

* Issuer/Audience validation

* Role-based authorization (User, Admin)

### Testing

* Full domain-level test coverage (BookTests, BookCategoryTests, UserBookTests, etc.)

* xUnit + FluentAssertions

* Domain entities created exclusively via factory methods (Create)

### Clean Architecture

The solution follows a strict multi-layer design:

Librium
│
- └── backend
   - ├── Librium.Domain          → Domain Models, DTOs, Interfaces
   - ├── Librium.Application     → Use Cases, Services, Business Logic
   - ├── Librium.Persistence     → EF Core, DbContext, Configurations, Repositories
   - ├── Librium.Identity        → JWT Token Service, Identity Infrastructure
   - ├── Librium.Presentation    → ASP.NET  Core API, Controllers, Swagger
   - └── Librium.Tests           → Unit Tests for Domain/Application

## Technologies Used
### Backend

* .NET 9 (ASP.NET  Core)

* C# 12

* EF Core 9

* JWT Authentication

* FluentValidation

* Moq (unit tests)

* xUnit + FluentAssertions

### Architecture

* Clean Architecture

* Domain-Driven Design

* Repository Pattern

* Dependency Injection

* DTO Mapping

* Domain events-ready structure
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

Librium uses production-grade automated pipelines with GitHub Actions:

### CI Pipeline (ci.yml)

Runs automatically on every pull request to master.

Includes:

* .NET 9 restore, build, test

* Domain + application test suite

* Docker image build

* Push to Azure Container Registry (ACR)

## CD Pipeline (cd.yml)

Runs automatically on every push to master.

Deploys image from ACR to Azure Web App for Containers.

## Domain Model Overview
## Book

Rich domain entity with strict invariants:
```
var result = Book.Create(title, author, categoryId, content, year);
```
Validations include:

* `title != null`
  
* `author != null`

* `categoryId != Guid.Empty`

* `content != null`

* `year >= 0`

## BookCategory

Created via:
```
var category = BookCategory.Create(name);
```
## AppUser

Inherits from IdentityUser and contains domain logic:
```
user.AddBook(bookId);
user.RemoveBook(bookId);
```
## UserBook

Join entity representing a book added by a user.

## Testing Coverage
* xUnit

* FluentAssertions

* Moq

Example Test (Book Create)
```
[Fact]
public void Create_ShouldReturnFailure_WhenTitleIsNull()
{
var result = Book.Create(null, "Author", Guid.NewGuid(), "Content", 2000);

result.IsSuccess.Should().BeFalse();
result.ErrorMessage.Should().Be("Title is required.");
}
```
Example Test (Book Update)
```
[Fact]
public void Update_ShouldReturnFailure_WhenContentIsNull()
{
var book = Book.Create("A", "B", Guid.NewGuid(), "C", 2000).Value!;
var category = BookCategory.Create("Sci").Value!;

var result = book.Update("New", "New", null, 2020, category);

result.IsSuccess.Should().BeFalse();
}
```
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

Protected controllers use:
```
[Authorize(Roles = UserRoles.User)]
```
## How to Run Tests
Run all tests:
```
dotnet test
```
Run only Book tests:
```
dotnet test --filter FullyQualifiedName~BookTests
```
## How to Run (5 Minutes!)
### 1. Clone the repo
```
git clone https://github.com/yourname/Librium.git
cd PristineIt
```
### 2. Set up SQL Server

Create a database: librium_db
Update connection string in appsettings.json
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
