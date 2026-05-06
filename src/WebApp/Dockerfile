FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY global.json .
COPY Directory.Build.props .
COPY Directory.Packages.props .

COPY src/WebApp/WebApp.csproj src/WebApp/
COPY src/WebAppComponents/WebAppComponents.csproj src/WebAppComponents/
COPY src/eShop.ServiceDefaults/eShop.ServiceDefaults.csproj src/eShop.ServiceDefaults/

RUN dotnet restore src/WebApp/WebApp.csproj

COPY src/ src/

RUN dotnet publish src/WebApp/WebApp.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends curl 
WORKDIR /app

RUN apt-get update && apt-get install -y ca-certificates
COPY nginx/certs/rootCA.pem /usr/local/share/ca-certificates/mkcert.crt
RUN update-ca-certificates

RUN apt-get update && apt-get install -y curl ca-certificates gnupg \
&& echo 'deb [signed-by=/etc/apt/keyrings/newrelic-apt.gpg] http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& curl -fsSL https://download.newrelic.com/NEWRELIC_APT_2DAD550E.public | gpg --dearmor > /etc/apt/keyrings/newrelic-apt.gpg \
&& apt-get update \
&& apt-get install -y newrelic-dotnet-agent

ENV NEW_RELIC_APP_NAME="WebApp"

COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebApp.dll"]
