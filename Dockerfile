# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project file first for caching
COPY planetapi.csproj .
RUN dotnet restore planetapi.csproj

# Copy everything else
COPY . .

# Publish
RUN dotnet publish planetapi.csproj -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "planetapi.dll"]
