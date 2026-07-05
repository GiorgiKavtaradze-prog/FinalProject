# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy project files
COPY FinalProject/FinalProjectApp/*.csproj ./FinalProjectApp/
COPY FinalProject/FinalProject.slnx ./

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY FinalProject/FinalProjectApp/. ./FinalProjectApp/

# Build and publish
WORKDIR /src/FinalProjectApp
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs

# Copy published application
COPY --from=build /app/publish .

# Set entry point
ENTRYPOINT ["dotnet", "FinalProjectApp.dll"]