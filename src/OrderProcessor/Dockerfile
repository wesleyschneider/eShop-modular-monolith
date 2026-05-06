FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json Directory.Build.props Directory.Build.targets Directory.Packages.props nuget.config ./

COPY src/Basket.API/Basket.API.csproj                             src/Basket.API/
COPY src/Catalog.API/Catalog.API.csproj                           src/Catalog.API/
COPY src/ClientApp/ClientApp.csproj                               src/ClientApp/
COPY src/EventBus/EventBus.csproj                                 src/EventBus/
COPY src/EventBusRabbitMQ/EventBusRabbitMQ.csproj                 src/EventBusRabbitMQ/
COPY src/HybridApp/HybridApp.csproj                               src/HybridApp/
COPY src/Identity.API/Identity.API.csproj                         src/Identity.API/
COPY src/IntegrationEventLogEF/IntegrationEventLogEF.csproj       src/IntegrationEventLogEF/
COPY src/OrderProcessor/OrderProcessor.csproj                     src/OrderProcessor/
COPY src/Ordering.API/Ordering.API.csproj                         src/Ordering.API/
COPY src/Ordering.Domain/Ordering.Domain.csproj                   src/Ordering.Domain/
COPY src/Ordering.Infrastructure/Ordering.Infrastructure.csproj   src/Ordering.Infrastructure/
COPY src/PaymentProcessor/PaymentProcessor.csproj                 src/PaymentProcessor/
COPY src/WebApp/WebApp.csproj                                     src/WebApp/
COPY src/WebAppComponents/WebAppComponents.csproj                 src/WebAppComponents/
COPY src/WebhookClient/WebhookClient.csproj                       src/WebhookClient/
COPY src/Webhooks.API/Webhooks.API.csproj                         src/Webhooks.API/
COPY src/eShop.AppHost/eShop.AppHost.csproj                       src/eShop.AppHost/
COPY src/eShop.ServiceDefaults/eShop.ServiceDefaults.csproj       src/eShop.ServiceDefaults/

RUN dotnet restore src/OrderProcessor/OrderProcessor.csproj

COPY src/ src/

RUN dotnet publish src/OrderProcessor/OrderProcessor.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update \
 && apt-get install -y --no-install-recommends curl ca-certificates \
 && rm -rf /var/lib/apt/lists/*

RUN apt-get update && apt-get install -y curl ca-certificates gnupg \
&& echo 'deb [signed-by=/etc/apt/keyrings/newrelic-apt.gpg] http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& curl -fsSL https://download.newrelic.com/NEWRELIC_APT_2DAD550E.public | gpg --dearmor > /etc/apt/keyrings/newrelic-apt.gpg \
&& apt-get update \
&& apt-get install -y newrelic-dotnet-agent

ENV NEW_RELIC_APP_NAME="OrderProcessor"

COPY nginx/certs/rootCA.pem /usr/local/share/ca-certificates/mkcert.crt
RUN update-ca-certificates

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessor.dll"]
