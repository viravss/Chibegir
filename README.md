# Chibegir

A .NET 9 solution built with Clean Architecture principles for comparing products.

## Architecture

This solution follows Clean Architecture principles with the following layers:

- **Chibegir.Domain**: Core business entities and domain logic
- **Chibegir.Application**: Application use cases, DTOs, and interfaces
- **Chibegir.Infrastructure**: Data access, external services, and infrastructure implementations
- **Chibegir.API**: Web API presentation layer with controllers and endpoints

## Project Structure

```
Chibegir/
├── Chibegir.Domain/
│   ├── Common/          # Base entities and common domain objects
│   └── Entities/         # Domain entities
├── Chibegir.Application/
│   ├── DTOs/            # Data Transfer Objects
│   ├── Interfaces/      # Application interfaces
│   └── Services/        # Application services
├── Chibegir.Infrastructure/
│   ├── Data/            # Repository implementations
│   └── DependencyInjection.cs  # DI configuration
└── Chibegir.API/
    └── Controllers/     # API controllers
```

## Getting Started

### Prerequisites

- .NET 9 SDK or later

### Building the Solution

```bash
dotnet build
```

### Running the API

```bash
cd Chibegir.API
dotnet run
```

The API will be available at `https://localhost:5001` or `http://localhost:5000` (depending on your configuration).

Swagger UI will be available at `/swagger` when running in Development mode.

## API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{category}` - Get products by category
- `POST /api/products` - Create a new product
- `PUT /api/products/{id}` - Update a product
- `DELETE /api/products/{id}` - Delete a product

## Clean Architecture Principles

1. **Dependency Rule**: Dependencies point inward. Domain has no dependencies, Application depends on Domain, Infrastructure and API depend on Application.

2. **Separation of Concerns**: Each layer has a specific responsibility:
   - Domain: Business logic and entities
   - Application: Use cases and business rules
   - Infrastructure: Technical implementation details
   - API: HTTP endpoints and request/response handling

3. **Independence**: The business logic is independent of frameworks, UI, and external concerns.

## Technology Stack

- .NET 9
- ASP.NET Core Web API
- Swagger/OpenAPI
- Clean Architecture

## License

This project is open source and available under the MIT License.
