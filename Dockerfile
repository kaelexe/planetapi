# ==========================
# BUILD STAGE
# ==========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project file first (for better build caching)
COPY planetapi.csproj ./

# Restore dependencies — this layer will be cached
# unless the project file changes
RUN dotnet restore planetapi.csproj

# Copy the rest of the application files
COPY . .

# Publish the app to the /app/publish folder
RUN dotnet publish planetapi.csproj -c Release -o /app/publish


# ==========================
# RUNTIME STAGE
# ==========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Expose default port (optional but good practice)
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "planetapi.dll"]
