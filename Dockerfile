# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "house-dashboard-server.dll"]

