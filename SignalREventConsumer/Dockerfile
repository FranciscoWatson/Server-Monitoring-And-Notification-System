# Use the Microsoft .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Since you're inside the project directory, adjust the path accordingly.
COPY ["SignalREventConsumer/SignalREventConsumer.csproj", "SignalREventConsumer/"]
RUN dotnet restore "SignalREventConsumer/SignalREventConsumer.csproj"

# Copy the rest of the source code
COPY . .

RUN dotnet build "SignalREventConsumer/SignalREventConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalREventConsumer/SignalREventConsumer.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SignalREventConsumer.dll"]
