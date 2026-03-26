# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY ["Bookanizer.REST.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Deploy stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS deploy
WORKDIR /app
COPY --from=build /app/publish .
RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser
EXPOSE 8080
ENTRYPOINT ["dotnet", "Bookanizer.REST.dll"]