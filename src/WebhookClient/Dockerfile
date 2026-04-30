FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY src/WebhookClient/WebhookClient.csproj src/WebhookClient/
COPY src/eShop.ServiceDefaults/eShop.ServiceDefaults.csproj src/eShop.ServiceDefaults/

RUN dotnet restore src/WebhookClient/WebhookClient.csproj

COPY src/ src/

RUN dotnet publish src/WebhookClient/WebhookClient.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebhookClient.dll"]
