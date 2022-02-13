# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim-arm32v7 AS build

# Set the working directory.
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0.14-buster-slim-arm32v7

WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:5000 
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "house-dashboard-server.dll"]
