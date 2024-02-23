## Blog API

The .NET Core API application was written using the .NET 8 preview version. The project was divided into layers to make good use of the Clean Architecture, Mediator and CQRS patterns.

### Layers in Clean Architecture
* Domain Layer - The domain layer lies in the center of the architecture where contains the enterprise logic, like the entities these are used to create the tables in the database.
* Application Layer - The application layer contains the business logic. In this layer that services interfaces are kept, separate from their implementation, for loose coupling and separation of concerns.
* Infrastructure Layer - In the infrastructure layer, are model objects, all the database migrations and database context Objects in this layer.
* Presentation layer - In this case, it's an API that accepts HTTP requests.

### The project used NuGet packages:
* MediatR 
* Swashbuckle.AspNetCore – slightly changes the appearance of the swagger during authorization
* FluentValidaƟon 
* AutoMapper 
* Serilog
* xUnit, Moq and Shouldly - needed for testing
And several packages from the EntityFrameworkCore and AspNetCore.Identity families.

The `IdentityDbContext` base class was used to create the Entity Framework database context, which contains the basic identity tables.
The `SaveChangesAsync` method has been overwritten in the context to save changes made to the database (Audit).

.NET 8 allows you to add user management API endpoints `app.MapGroup("/account").MapIdentityApi();` however, using this method you can add "either all or nothing", which is why a separate controller for registration and login was created, which will also allow sending more information about the user during registration to the `ApplicationUser` model.

### In middleware are added four IPipelineBehavior:
* UnhandledExceptionBehavior - Error handler
* ValidationBehavior - Validate request
* LoggingBehavior - Logging
* PerformanceBehavior - Logging if request is processed to long
* CacheBehavior - Memory cache

## Important
#### The project dont use the repository pattern so after move database model configuration to separate files, tests stopped working beouse i'm used SQL Lite to make fake database. SQL Lite dont support nvarchar what make errors.
