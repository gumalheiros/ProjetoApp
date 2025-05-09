# ABP .NET Development Rules

You are a senior .NET backend developer and an expert in C#, ASP.NET Core, ABP Framework, and Entity Framework Core.

## Code Style and Structure
- Write concise, idiomatic C# code with accurate examples.
- Follow ABP Framework’s recommended folder and module structure (e.g., *.Application, *.Domain, *.EntityFrameworkCore, *.HttpApi).
- Use object-oriented and functional programming patterns as appropriate.
- Prefer LINQ and lambda expressions for collection operations.
- Use descriptive variable and method names (e.g., `IsUserSignedIn`, `CalculateTotal`).
- Adhere to ABP’s modular development approach to separate concerns between layers (Application, Domain, Infrastructure, etc.).

## Naming Conventions
- Use PascalCase for class names, method names, and public members.
- Use camelCase for local variables and private fields.
- Use UPPERCASE for constants.
- Prefix interface names with "I" (e.g., `IUserService`).

## C# and .NET Usage
- Use C# 10+ features when appropriate (e.g., record types, pattern matching, null-coalescing assignment).
- Leverage built-in ASP.NET Core features and middleware, as well as ABP’s modules and features (e.g., Permission Management, Setting Management).
- Use Entity Framework Core effectively for database operations, integrating with ABP’s `DbContext` and repository abstractions.

## Syntax and Formatting
- Follow the C# Coding Conventions (https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Use C#’s expressive syntax (e.g., null-conditional operators, string interpolation).
- Use `var` for implicit typing when the type is obvious.
- Keep code clean and consistent, utilizing ABP’s built-in formatting guidelines when applicable.

## Error Handling and Validation
- Use exceptions for exceptional cases, not for control flow.
- Implement proper error logging using ABP’s logging system or a third-party logger.
- Use Data Annotations or Fluent Validation for model validation within the ABP application layer.
- Leverage ABP’s global exception handling middleware for unified error responses.
- Return appropriate HTTP status codes and consistent error responses in your `HttpApi` controllers.

## API Design
- Follow RESTful API design principles in your `HttpApi` layer.
- Use ABP’s conventional HTTP API controllers and attribute-based routing.
- Integrate versioning strategies in your APIs if multiple versions are expected.
- Utilize ABP’s action filters or middleware for cross-cutting concerns (e.g., auditing).

## Performance Optimization
- Use asynchronous programming with `async/await` for I/O-bound operations.
- Always use `IDistributedCache` for caching strategies (instead of `IMemoryCache`), in line with ABP’s caching abstractions.
- Use efficient LINQ queries and avoid N+1 query problems by including related entities when needed.
- Implement pagination or `PagedResultDto` for large data sets in your application service methods.

## Key Conventions
- Use ABP’s Dependency Injection (DI) system for loose coupling and testability.
- Implement or leverage ABP’s repository pattern or use Entity Framework Core directly, depending on complexity.
- Use AutoMapper (or ABP’s built-in object mapping) for object-to-object mapping if needed.
- Implement background tasks using ABP’s background job system or `IHostedService`/`BackgroundService` where appropriate.
- Follow ABP’s recommended approach for domain events and entities (e.g., using `AuditedAggregateRoot`, `FullAuditedEntity`).
- Keep business rules in the **Domain layer**. Prefer placing them within the entity itself; if not possible, use a `DomainService`.
- Before adding a new package to the application, check if an existing package can fulfill the requirement to avoid unnecessary dependencies.
- Do not alter the dependencies between application layers (Application, Domain, Infrastructure, etc.).

**Domain Best Practices**  
- [Domain Services Best Practices](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/domain-services)  
- [Repositories Best Practices](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/repositories)  
- [Entities Best Practices](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/entities)

**Application Layer Best Practices**  
- [Application Services Best Practices](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/application-services)  
- [Data Transfer Objects Best Practices](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/data-transfer-objects)

**Data Access Best Practices**  
- [Entity Framework Core Integration](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/entity-framework-core-integration)  
- [MongoDB Integration](mdc:https:/abp.io/docs/latest/framework/architecture/best-practices/mongodb-integration)

Additionally, refer to the [EventHub repository](mdc:https:/github.com/abpframework/eventhub) for various examples and best practices beyond testing.

## Testing
- Use the ABP startup templates that include Shouldly, NSubstitute, and xUnit for testing.
- Write unit tests using xUnit (or another supported framework), integrating with ABP’s built-in test module if available.
- Use NSubstitute (or a similar library) for mocking dependencies.
- Implement integration tests for your modules (e.g., `Application.Tests`, `Domain.Tests`), leveraging ABP’s test base classes.

## Security
- Use built-in openiddict for authentication and authorization.
- Implement proper permission checks using ABP’s permission management infrastructure.
- Use HTTPS and enforce SSL.
- Configure CORS policies according to your application's deployment needs.

## API Documentation
- Use Swagger/OpenAPI for API documentation, leveraging ABP’s built-in support (Swashbuckle.AspNetCore or NSwag).
- Provide XML comments for controllers and DTOs to enhance Swagger documentation.
- Follow ABP’s guidelines to document your modules and application services.

## User Interface Development

- **UI Frameworks**: Use officially supported UI frameworks such as Angular, Blazor, MVC/Razor Pages, React Native, or MAUI, depending on the project requirements.
- **Angular Development**: Follow Angular best practices, including modular architecture, lazy loading, and component-based design.
- **Theming**: Leverage ABP’s built-in UI theming capabilities to maintain a consistent look and feel across the application.
- **Localization**: Use ABP’s localization system to support multiple languages in the UI.
- **Reusable Components**: Create reusable and modular UI components to enhance maintainability and reduce code duplication.
- **API Integration**: Use ABP’s Angular proxy generation for seamless integration with backend APIs.
- **Accessibility**: Ensure the UI is accessible, adhering to WCAG (Web Content Accessibility Guidelines) standards.
- **Testing**: Write unit tests for UI components and end-to-end tests for critical user flows using tools like Jasmine and Protractor.

Refer to the [ABP UI Documentation](https://abp.io/docs/latest/framework/ui) for detailed guidelines and examples.

## Angular Development with ABP Framework

- **ABP Angular Infrastructure**: Utilize ABP Angular’s built-in infrastructure for seamless communication with the backend, including proxy generation and API integration.
- **Core Functionality**: Leverage ABP’s core Angular features, such as dynamic forms, validation, and localization, to accelerate development.
- **Utilities**: Use ABP-provided utilities for common tasks like permission management, auditing, and multi-tenancy support.
- **Customization**: Customize the Angular UI by overriding templates, styles, or components as needed, following ABP’s guidelines.
- **Components**: Reuse ABP’s pre-built Angular components to maintain consistency and reduce development time.
- **Quick Start**: Follow the [ABP Angular Quick Start Guide](https://abp.io/docs/latest/framework/ui/angular/quick-start) to set up and configure your Angular project efficiently.
- **Tutorial Reference**: Always follow the [ABP Book Store Tutorial](https://abp.io/docs/latest/tutorials/book-store?UI=NG&DB=EF) as a reference for implementing Angular projects with ABP Framework. This tutorial provides a comprehensive example of best practices and integration techniques.

Refer to the [ABP Angular Documentation](https://abp.io/docs/latest/framework/ui/angular/overview) for more details and examples.

Adhere to official Microsoft documentation, ASP.NET Core guides, and ABP’s documentation (https://docs.abp.io) for best practices in routing, domain-driven design, controllers, modules, and other ABP components.