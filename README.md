# DockerPackaging

A .NET 8 Web API that exposes CRUD operations for a Song entity using Entity Framework Core, running inside Docker Compose with PostgreSQL.

## 🏗️ Project Structure

```
DockerPackaging/
├── src/                          # API project
│   ├── Models/                   # Entity models
│   │   └── Song.cs              # Song entity with Id, Title, Artist, ReleaseDate
│   ├── Data/                     # Data access layer
│   │   ├── ApplicationDbContext.cs    # EF Core DbContext
│   │   ├── SeedData.cs          # Sample data seeding
│   │   └── DbContextExtensions.cs     # Database initialization helpers
│   ├── Services/                 # Business logic layer
│   │   ├── ISongService.cs      # Song service interface
│   │   └── SongService.cs       # Song service implementation
│   ├── Controllers/              # API controllers
│   │   └── SongsController.cs   # RESTful CRUD endpoints
│   ├── Program.cs                # Application entry point
│   ├── appsettings.json         # Configuration
│   └── Dockerfile               # Docker container definition
├── tests/                        # Test project
│   ├── Services/                 # Service unit tests
│   ├── Controllers/              # Controller unit tests
│   └── TestHelpers/              # Test utilities
├── docker-compose.yml            # Multi-container orchestration
└── DockerPackaging.sln           # Solution file
```

## 🚀 Features

- **Full CRUD Operations** for Song entity
- **Entity Framework Core** with PostgreSQL
- **Docker & Docker Compose** deployment
- **Comprehensive Unit Testing** with xUnit, Moq, and FluentAssertions
- **Automatic Database Seeding** with sample data
- **Swagger as Primary API Documentation** - Comprehensive, interactive API reference
- **Clean Architecture** with separation of concerns

## 🎵 Song Entity

```csharp
public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
}
```

## 🗄️ Database

- **PostgreSQL 15** running in Docker container
- **Automatic migrations** and database creation
- **Sample data** seeded on startup including:
  - De La Soul songs (Me Myself and I, The Magic Number, Buddy)
  - TimeFlies songs (I Choose You, Just a Little Bit, Turn Back Time)
  - Killing Heidi songs (Weir, Mascara, Superman)

## 🐳 Docker Deployment

### Prerequisites
- Docker Desktop
- Docker Compose

### Quick Start
```bash
# Build and start all services
docker compose up --build -d

# View logs
docker compose logs -f

# Stop services
docker compose down
```

**📚 API Documentation**: Once running, visit http://localhost:5000/swagger for complete API documentation and interactive testing.

### Services
- **API**: .NET 8 Web API running on port 5000
- **PostgreSQL**: Database running on port 5432

### Access Points
- **API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger
- **Database**: localhost:5432 (postgres/postgres)

## 📡 API Endpoints

The API provides full CRUD operations for managing songs. All endpoints are documented with comprehensive examples, request/response schemas, and validation rules in the **Swagger UI**.

### **Quick Access**
- **Swagger Documentation**: http://localhost:5000/swagger
- **API Base URL**: http://localhost:5000/api

### **Available Operations**
- **GET** `/api/songs` - Retrieve all songs
- **GET** `/api/songs/{id}` - Get song by ID  
- **POST** `/api/songs` - Create new song
- **PUT** `/api/songs/{id}` - Update existing song
- **DELETE** `/api/songs/{id}` - Delete song by ID

### **Why Use Swagger?**
- **Interactive Testing**: Try endpoints directly from the browser
- **Live Examples**: See exact request/response formats
- **Validation Rules**: Understand required fields and constraints
- **Error Handling**: Learn about all possible response codes
- **Always Up-to-Date**: Documentation automatically syncs with code

## 📚 Enhanced Swagger Documentation

**Swagger UI is your primary source for complete API documentation.** It provides comprehensive, interactive documentation that automatically stays in sync with your code.

### **What You'll Find in Swagger**

#### **Complete API Reference**
- **All Endpoints**: Detailed documentation for every API operation
- **Request/Response Examples**: Real-world examples for testing
- **Parameter Validation**: Required fields, data types, and constraints
- **Response Models**: Complete schema definitions for all data types
- **Error Handling**: All possible HTTP status codes and error messages

#### **Interactive Features**
- **Live Testing**: Execute API calls directly from the browser
- **Request Builder**: Easy-to-use forms for building requests
- **Response Viewer**: See actual API responses in real-time
- **Schema Explorer**: Browse and understand data models

### **Access Swagger UI**
Navigate to **http://localhost:5000/swagger** to explore the complete API documentation.

### **Key Benefits**
- ✨ **Always Current**: Documentation automatically updates with code changes
- 🔍 **Search & Filter**: Easy navigation through all endpoints
- 📱 **Responsive Design**: Works perfectly on all devices
- 🎯 **Deep Linking**: Direct links to specific endpoints
- ⚡ **Performance Metrics**: See request duration and response times
- 📊 **Visual Schemas**: Clear visualization of data structures

## 🧪 Testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run specific test project
dotnet test tests/
```

### Test Coverage
- **Service Tests**: 15 tests covering all CRUD operations and validation
- **Controller Tests**: 10 tests covering HTTP responses and service integration
- **Total**: 25 tests with 100% pass rate

### Test Technologies
- **xUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Readable assertions
- **EF Core InMemory** - In-memory database for testing

## 🛠️ Development

### Prerequisites
- .NET 8 SDK
- Docker Desktop

### Local Development
```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Run API locally (requires PostgreSQL)
dotnet run --project src/
```

### Project Structure Benefits
- **Separation of Concerns**: Models, Services, Controllers clearly separated
- **Testability**: Services and Controllers easily unit tested
- **Maintainability**: Clean, organized code structure
- **Scalability**: Easy to add new entities and features
- **Documentation**: Comprehensive XML comments for enhanced Swagger

### XML Documentation
The project generates XML documentation files that are automatically integrated with Swagger:
- All public methods and properties are documented
- Examples and remarks provide additional context
- Swagger UI displays all documentation automatically
- **Single Source of Truth**: All API documentation is maintained in Swagger, not duplicated in README

## 🔧 Configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ASPNETCORE_URLS`: HTTP binding (default: http://+:80)

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=songsdb;Username=postgres;Password=postgres"
  }
}
```

## 📝 Error Handling

The API returns appropriate HTTP status codes. For complete error handling documentation, including all status codes and error messages, see the **Swagger UI** at http://localhost:5000/swagger.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add/update tests
5. Ensure all tests pass
6. Submit a pull request

**📝 Documentation**: When adding new API endpoints, ensure they include comprehensive XML comments for automatic Swagger documentation.

## 📄 License

This project is licensed under the MIT License.