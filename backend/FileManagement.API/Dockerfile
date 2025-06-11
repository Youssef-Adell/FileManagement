# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy project files and restore first to take advantage of docker layer caching
COPY FileManagement.sln ./
COPY FileManagement.API/FileManagement.API.csproj FileManagement.API/
COPY FileManagement.Business/FileManagement.Business.csproj FileManagement.Business/
COPY FileManagement.Data/FileManagement.Data.csproj FileManagement.Data/

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source code and build/publish the Web API
COPY . .
RUN dotnet publish FileManagement.API/FileManagement.API.csproj -c Release -o /publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /release

# Copy published files from the build stage
COPY --from=build /publish ./

# Run the API
ENTRYPOINT ["dotnet", "FileManagement.API.dll"]
