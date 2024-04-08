# Define the base image for running the application
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["MessageProcessingService/MessageProcessingService.csproj", "MessageProcessingService/"]
COPY ["MessagingQueueLibrary/MessagingQueueLibrary.csproj", "MessagingQueueLibrary/"]
RUN dotnet restore "MessageProcessingService/MessageProcessingService.csproj"

# Copy everything else and build the project
COPY . .
WORKDIR "/src/MessageProcessingService"
RUN dotnet build "MessageProcessingService.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MessageProcessingService.csproj" -c Release -o /app/publish

# Build the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageProcessingService.dll"]
