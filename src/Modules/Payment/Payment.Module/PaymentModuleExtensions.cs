using eShop.Payment.Module;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class PaymentModuleExtensions
{
    public static IHostApplicationBuilder AddPaymentModule(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<PaymentOptions>()
            .BindConfiguration(nameof(PaymentOptions));

        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(PaymentModuleExtensions).Assembly));

        return builder;
    }
}
