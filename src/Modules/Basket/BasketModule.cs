using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket
{
    public static class BasketModule
    {
        public static WebApplicationBuilder AddBasketModule(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            //Add services to the container

            // Api Endpoint services

            // Application use case services

            // Data - Infrastructure services
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            builder.AddNpgsqlDbContext<BasketDbContext>("EshopDb", (settings) => {
            }, (optionsBuilder) =>
            {
                var sp = builder.Services.BuildServiceProvider();
                optionsBuilder.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            });
            return builder;
        }
        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            //Configure the http request pipeline.

            // 1. Use Api Endpoint services

            // 2. Use Application Use Case services

            // 3. Use Data - Infrasturcture services
            app.UseMigration<BasketDbContext>();
            return app;
        }
    }
}
