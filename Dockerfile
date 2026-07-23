# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY ["StudifyAPI/StudifyAPI.csproj", "StudifyAPI/"]
# You can add other projects here if needed, but for now we copy everything in the next step

# Restore dependencies
RUN dotnet restore "StudifyAPI/StudifyAPI.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR "/src/StudifyAPI"
RUN dotnet publish "StudifyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port (Render overrides this with PORT env var, but it's good practice)
EXPOSE 8080

ENTRYPOINT ["dotnet", "StudifyAPI.dll"]