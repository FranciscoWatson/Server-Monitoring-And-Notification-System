FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SignalRApp/SignalRApp.csproj", "SignalRApp/"]
RUN dotnet restore "SignalRApp/SignalRApp.csproj"
COPY . .
RUN dotnet build "SignalRApp/SignalRApp.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "SignalRApp/SignalRApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalRApp.dll"]
