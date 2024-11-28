using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Catalog
{
    public static class CatalogModule
    {
        public static WebApplicationBuilder AddCatalogModule(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            //Add services to the container

            // Api Endpoint services

            // Application use case services

            // Data - Infrastructure services
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            builder.AddNpgsqlDbContext<CatalogDbContext>("EshopDb", (settings) => {
            }, (optionsBuilder) =>
            {
                var sp = builder.Services.BuildServiceProvider();
                optionsBuilder.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            });
            builder.Services.AddScoped<IDataSeeder, CatalogDataSeeder>();
            return builder;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            //Configure the http request pipeline.

            // 1. Use Api Endpoint services

            // 2. Use Application Use Case services

            // 3. Use Data - Infrasturcture services
            app.UseMigration<CatalogDbContext>();
            return app;
        }
    }
}
