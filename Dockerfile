# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy project files
COPY ["EmployeeManagement/EmployeeManagement.API.csproj", "EmployeeManagement/"]
COPY ["EmployeeManagement.Application/EmployeeManagement.Application.csproj", "EmployeeManagement.Application/"]
COPY ["EmployeeManagement.Domain/EmployeeManagement.Domain.csproj", "EmployeeManagement.Domain/"]
COPY ["EmployeeManagement.Infrastructure/EmployeeManagement.Infrastructure.csproj", "EmployeeManagement.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "EmployeeManagement/EmployeeManagement.API.csproj"

# Copy source code
COPY . .

# Build
WORKDIR "/src/EmployeeManagement"

RUN dotnet build "EmployeeManagement.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish

RUN dotnet publish "EmployeeManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "EmployeeManagement.API.dll"]