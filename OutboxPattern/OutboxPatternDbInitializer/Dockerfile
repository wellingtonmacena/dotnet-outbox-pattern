# Imagem base para build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia csproj e restaura dependências
COPY OutboxPatternDbInitializer.csproj ./
RUN dotnet restore

# Copia tudo e publica
COPY . .
RUN dotnet publish -c Release -o /app

# Imagem final para rodar
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OutboxPatternDbInitializer.dll"]
