FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY src/eShop.ApiHost/eShop.ApiHost.csproj src/eShop.ApiHost/
COPY src/eShop.ServiceDefaults/eShop.ServiceDefaults.csproj src/eShop.ServiceDefaults/
COPY src/eShop.SharedKernel/eShop.SharedKernel.csproj src/eShop.SharedKernel/
COPY src/Modules/Catalog/Catalog.Module/Catalog.Module.csproj src/Modules/Catalog/Catalog.Module/
COPY src/Modules/Basket/Basket.Module/Basket.Module.csproj src/Modules/Basket/Basket.Module/
COPY src/Modules/Identity/Identity.Module/Identity.Module.csproj src/Modules/Identity/Identity.Module/
COPY src/Modules/Ordering/Ordering.Module/Ordering.Module.csproj src/Modules/Ordering/Ordering.Module/
COPY src/Modules/Ordering/Ordering.Domain/Ordering.Domain.csproj src/Modules/Ordering/Ordering.Domain/
COPY src/Modules/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj src/Modules/Ordering/Ordering.Infrastructure/
COPY src/Modules/Ordering/Ordering.Contracts/Ordering.Contracts.csproj src/Modules/Ordering/Ordering.Contracts/
COPY src/Modules/Payment/Payment.Module/Payment.Module.csproj src/Modules/Payment/Payment.Module/
COPY src/Modules/Webhooks/Webhooks.Module/Webhooks.Module.csproj src/Modules/Webhooks/Webhooks.Module/

RUN dotnet restore src/eShop.ApiHost/eShop.ApiHost.csproj

COPY src/ src/

RUN dotnet publish src/eShop.ApiHost/eShop.ApiHost.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends curl 
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "eShop.ApiHost.dll"]
