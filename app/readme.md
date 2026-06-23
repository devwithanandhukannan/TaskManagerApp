# Database
##  Install MongoDB EF Core 
```
cd TaskManager.Infrastructure
dotnet add package MongoDB.EntityFrameworkCore
```

# Solution File
```
dotnet new sln -n TaskManager

dotnet sln add TaskManager.Domain/TaskManager.Domain.csproj
dotnet sln add TaskManager.Application/TaskManager.Application.csproj
dotnet sln add TaskManager.Infrastructure/TaskManager.Infrastructure.csproj
dotnet sln add TaskManager.API/TaskManager.API.csproj

dotnet sln list
```

