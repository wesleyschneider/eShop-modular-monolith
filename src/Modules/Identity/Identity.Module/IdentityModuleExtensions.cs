using eShop.Identity.Module.Configuration;
using eShop.Identity.Module.Data;
using eShop.Identity.Module.Models;
using eShop.Identity.Module.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.Hosting;

public static class IdentityModuleExtensions
{
    public static IHostApplicationBuilder AddIdentityModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        builder.AddNpgsqlDbContext<ApplicationDbContext>("eshopdb");
        builder.Services.AddMigration<ApplicationDbContext, UsersSeed>();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
        {
            options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.KeyManagement.Enabled = false;
        })
        .AddInMemoryIdentityResources(Config.GetResources())
        .AddInMemoryApiScopes(Config.GetApiScopes())
        .AddInMemoryApiResources(Config.GetApis())
        .AddInMemoryClients(Config.GetClients(builder.Configuration))
        .AddAspNetIdentity<ApplicationUser>()
        .AddDeveloperSigningCredential();

        builder.Services.AddTransient<IProfileService, ProfileService>();
        builder.Services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
        builder.Services.AddTransient<IRedirectService, RedirectService>();

        return builder;
    }

    public static WebApplication UseIdentityModule(this WebApplication app)
    {
        var embeddedProvider = new ManifestEmbeddedFileProvider(
            typeof(IdentityModuleExtensions).Assembly,
            "wwwroot");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = embeddedProvider
        });

        app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });
        app.UseIdentityServer();
        return app;
    }

    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDefaultControllerRoute();
        return app;
    }
}
