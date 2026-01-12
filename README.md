# REST Gateway API

A REST Gateway API that acts as an intermediary between REST clients and a SOAP-based Order Service.

## Features

- RESTful API
- SOAP 1.1 client integration (BasicHttpBinding)
- Comprehensive error handling
- Request validation using FluentValidation
- OpenAPI/Swagger documentation
- Environment-specific configuration
- Docker support
- Health check endpoints
- Structured logging

## Prerequisites

- .NET 9.0 SDK
- Docker (optional, for containerized deployment)
- Access to Order Service SOAP endpoint

## Getting Started

### Local Development

1. Clone the repository
2. Update `appsettings.Development.json` with your Order Service endpoint URL
3. Run the application:

```bash
dotnet run
```

4. Access Swagger UI at: `http://localhost:5002/swagger` (or the configured port)

### Docker

#### Development

```bash
docker-compose up
```

#### Production

```bash
docker-compose -f docker-compose.prod.yml up -d
```

## Configuration

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Set to `Development`, `Staging`, or `Production`
- `OrderService__BaseUrl`: SOAP service endpoint URL

### AppSettings

Configuration files are available for different environments:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development settings
- `appsettings.Staging.json` - Staging settings
- `appsettings.Production.json` - Production settings

## API Endpoints

### Health Check

- `GET /health` - Health check endpoint

### Orders API (v1)

- `POST /api/v1/orders` - Create a new order
- `GET /api/v1/orders/{orderId}` - Get order details
- `GET /api/v1/orders/{orderId}/total` - Calculate order total
- `PUT /api/v1/orders/{orderId}/status` - Update order status

## API Versioning

The API supports versioning through URL path:

- `/api/v1/orders` - Version 1.0

## Error Handling

The API returns standardized error responses:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "Validation error message",
  "traceId": "00-...",
  "extensions": {}
}
```

## Logging

Logging is configured per environment:

- **Development**: Debug level
- **Staging**: Information level
- **Production**: Warning level

## Deployment

### Docker

Build and run:

```bash
docker build -t restgateway .
docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production restgateway
```

### Kubernetes

See deployment manifests in `k8s/` directory (if available).

## Development

### Project Structure

```
RestGateway/
├── Controllers/          # API controllers
│   ├── V1/              # Version 1 controllers
│   └── HealthController.cs
├── Models/              # Data models
│   └── DTOs/           # Data Transfer Objects
├── Services/            # Business logic services
├── Mappings/           # DTO to SOAP mappers
├── Middleware/         # Custom middleware
├── Validators/         # FluentValidation validators
└── Program.cs          # Application entry point
```

## Testing

Run tests:

```bash
dotnet test
```
