FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FileManagement.API/FileManagement.API.csproj", "FileManagement.API/"]
COPY ["FileManagement.Business/FileManagement.Business.csproj", "FileManagement.Business/"]
COPY ["FileManagement.Data/FileManagement.Data.csproj", "FileManagement.Data/"]
RUN dotnet restore "FileManagement.API/FileManagement.API.csproj"
COPY . .
WORKDIR "/src/FileManagement.API"
RUN dotnet build "FileManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileManagement.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileManagement.API.dll"] 