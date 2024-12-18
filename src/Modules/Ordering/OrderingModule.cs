using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering
{
    public static class OrderingModule
    {
        public static WebApplicationBuilder AddOrderingModule(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            // Add services to the container.
            // 1. Api Endpoint services

            // 2. Application Use Case services        

            // 3. Data - Infrastructure services
            var connectionString = configuration.GetConnectionString("Database");

            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableInterceptor>();
            builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            builder.AddNpgsqlDbContext<OrderingDbContext>("EshopDb", (settings) => {
            }, (optionsBuilder) =>
            {
                var sp = builder.Services.BuildServiceProvider();
                optionsBuilder.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            });
            return builder;
        }

        public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline.
            // 1. Use Api Endpoint services

            // 2. Use Application Use Case services

            // 3. Use Data - Infrastructure services
            app.UseMigration<OrderingDbContext>();

            return app;
        }
    }
}
