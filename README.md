# ![RealWorld Example App](logo.png)

> ### ASP.NET Core codebase containing real world examples (CRUD, auth, advanced patterns, etc) that adheres to the [RealWorld](https://github.com/gothinkster/realworld) spec and API.

This codebase was created to demonstrate a fully fledged fullstack application built with ASP.NET Core including CRUD
operations, authentication, routing, pagination, and more.

We've gone to great lengths to adhere to the ASP.NET Core community style guides & best practices.

For more information on how to this works with other frontends/backends, head over to
the [RealWorld](https://github.com/gothinkster/realworld) repo.

## How it works

Aside from [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0), numerous
technologies are used within this solution including:

- Data access with [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- CQRS with [MediatR](https://github.com/jbogard/MediatR)
- Validation with [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- Object mapping with [AutoMapper](https://github.com/AutoMapper/AutoMapper)
- Automated testing with [xUnit](https://xunit.net/), [FluentAssertions](https://fluentassertions.com/),
  and [Moq](https://github.com/devlooped/moq)
- Built-in Swagger via [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

The architecture and design of this project is based on
the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) design pattern.
The application is broken down into multi-project solutions, where each project is considered to reside in a particular
layer of the application.

### The Core Project

The Core project is the center of the Clean Architecture design, and all other project dependencies should point toward
it. As such, it has very few external dependencies and should not be dependent on data access and other infrastructure
concerns so those dependencies are inverted. This is achieved by adding interfaces or abstractions within Core that are
implemented by layers outside of Core.

### The Application Project

Often referred to as the Use Case layer, the Application project contains all the business logic for the application.
This project implements CQRS (Command Query Responsibility Segregation), with each business use case represented by a
single command or query.

### The Infrastructure Project

Application's dependencies on external resources should be implemented here, in classes defined in the Infrastructure
project. These classes should implement interfaces defined in the Core project.

> In a typical ASP.NET Core web application, these implementations include the Entity Framework (EF) DbContext, any EF
> Core Migration objects that have been defined, and data access implementation classes. The most common way to abstract
> data access implementation code is through the use of the [Repository](https://deviq.com/repository-pattern/) design
> pattern.

### The Web Project

The entry point of the application is the ASP.NET Core web project. The `Program.cs` file is responsible for configuring
the application and wiring up implementation types to interfaces.

> In order to wire up dependency injection during app startup, the `Program.cs` file may need to reference the
> Infrastructure project.

## Getting started

The solution-level [`docker-compose.yml`](./docker-compose.yml) file is configured to run the application in a
containerized environment. To run
the application, execute the following command from the root of the repository:

```bash
docker-compose up
```

The application will be available at <http://localhost:5000/swagger>.
