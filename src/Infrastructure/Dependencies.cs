using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NimblePros.Metronome;

namespace Microsoft.eShopWeb.Infrastructure;

public static class Dependencies
{
    public static void ConfigureLocalDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        bool useOnlyInMemoryDatabase = false;
        if (configuration["UseOnlyInMemoryDatabase"] != null)
        {
            useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);
        }

        if (useOnlyInMemoryDatabase)
        {
            services.AddDbContext<CatalogContext>((provider, options) =>
                options.UseInMemoryDatabase("Catalog"));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase("Identity"));
        }
        else
        {
            // Catalog DbContext
            string catalogConnectionString = configuration.GetConnectionString("CatalogConnection")!;
            services.AddDbContext<CatalogContext>((provider, options) =>
            {
                options.UseSqlServer(catalogConnectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null))
                    .AddInterceptors(provider.GetRequiredService<DbCallCountingInterceptor>());
            });

            // Identity DbContext
            string identityConnectionString = configuration.GetConnectionString("IdentityConnection")!;
            services.AddDbContext<AppIdentityDbContext>((provider, options) =>
            {
                options.UseSqlServer(identityConnectionString, sqlOptions =>
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null));
                options.AddInterceptors(provider.GetRequiredService<DbCallCountingInterceptor>());
            });
        }
    }
}