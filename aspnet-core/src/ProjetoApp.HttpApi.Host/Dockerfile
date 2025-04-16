FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0.203 AS build
WORKDIR /src

# Copiar todo o código fonte de uma vez
COPY . .

# Adicionar diretamente os pacotes do Entity Framework Core ao projeto Application
WORKDIR "/src"
RUN dotnet add src/ProjetoApp.Application/ProjetoApp.Application.csproj package Microsoft.EntityFrameworkCore --version 9.0.0
RUN dotnet add src/ProjetoApp.Application/ProjetoApp.Application.csproj package Microsoft.EntityFrameworkCore.Relational --version 9.0.0
RUN dotnet add src/ProjetoApp.Application/ProjetoApp.Application.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0

# Restaurar todos os pacotes
RUN dotnet restore src/ProjetoApp.HttpApi.Host/ProjetoApp.HttpApi.Host.csproj --disable-parallel

# Publicar o projeto
WORKDIR "/src/src/ProjetoApp.HttpApi.Host"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetoApp.HttpApi.Host.dll"]