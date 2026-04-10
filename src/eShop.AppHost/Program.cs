using eShop.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddForwardedHeaders();

var redis = builder.AddRedis("redis");
var postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent);

var eshopDb = postgres.AddDatabase("eshopdb");

var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

// Single API host
var apiHost = builder.AddProject<Projects.eShop_ApiHost>("api", launchProfileName)
    .WithExternalHttpEndpoints()
    .WithReference(eshopDb).WaitFor(eshopDb)
    .WithReference(redis)
    .WithHttpHealthCheck("/health");

var apiEndpoint = apiHost.GetEndpoint(launchProfileName);

// Reverse proxies
builder.AddYarp("mobile-bff")
    .WithExternalHttpEndpoints()
    .ConfigureMobileBffRoutes(apiHost);

// Apps
var webhooksClient = builder.AddProject<Projects.WebhookClient>("webhooksclient", launchProfileName)
    .WithReference(apiHost)
    .WithEnvironment("IdentityUrl", apiEndpoint);

var webApp = builder.AddProject<Projects.WebApp>("webapp", launchProfileName)
    .WithExternalHttpEndpoints()
    .WithUrls(c => c.Urls.ForEach(u => u.DisplayText = $"Online Store ({u.Endpoint?.EndpointName})"))
    .WithReference(apiHost)
    .WaitFor(apiHost)
    .WithEnvironment("IdentityUrl", apiEndpoint);

// set to true if you want to use OpenAI
bool useOpenAI = false;
if (useOpenAI)
{
    builder.AddOpenAI(apiHost, webApp, OpenAITarget.OpenAI); // set to AzureOpenAI if you want to use Azure OpenAI
}

bool useOllama = false;
if (useOllama)
{
    builder.AddOllama(apiHost, webApp);
}

// Wire up the callback urls (self referencing)
webApp.WithEnvironment("CallBackUrl", webApp.GetEndpoint(launchProfileName));
webhooksClient.WithEnvironment("CallBackUrl", webhooksClient.GetEndpoint(launchProfileName));

// Identity callback URLs — now all within the ApiHost
apiHost.WithEnvironment("BasketApiClient", apiHost.GetEndpoint("http"))
       .WithEnvironment("OrderingApiClient", apiHost.GetEndpoint("http"))
       .WithEnvironment("WebhooksApiClient", apiHost.GetEndpoint("http"))
       .WithEnvironment("WebhooksWebClient", webhooksClient.GetEndpoint(launchProfileName))
       .WithEnvironment("WebAppClient", webApp.GetEndpoint(launchProfileName));

builder.Build().Run();

// For test use only.
// Looks for an environment variable that forces the use of HTTP for all the endpoints. We
// are doing this for ease of running the Playwright tests in CI.
static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "ESHOP_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
