FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ServerStatisticsCollectionService/ServerStatisticsCollectionService.csproj", "ServerStatisticsCollectionService/"]
COPY ["MessagingQueueLibrary/MessagingQueueLibrary.csproj", "MessagingQueueLibrary/"]
RUN dotnet restore "ServerStatisticsCollectionService/ServerStatisticsCollectionService.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/ServerStatisticsCollectionService"
RUN dotnet build "ServerStatisticsCollectionService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ServerStatisticsCollectionService.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerStatisticsCollectionService.dll"]
