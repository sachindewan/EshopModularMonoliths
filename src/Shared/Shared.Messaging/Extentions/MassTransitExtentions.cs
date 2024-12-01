using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions;
public static class MassTransitExtentions
{
    public static IServiceCollection AddMassTransitWithAssemblies
        (this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);
            //config.AddSagaStateMachines(assemblies);
            //config.AddSagas(assemblies);
            //config.AddActivities(assemblies);

            config.UsingRabbitMq((context, configurator) =>
            {
                var connectionString = configuration.GetConnectionString("messageBroker")!;

                configurator.Host(new Uri(connectionString!), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });
                //configurator.Host(configuration.GetConnectionString("messageBroker"), host =>
                //{
                //    host.Username(configuration["rabbitUserName"]!);
                //    host.Password(configuration["rabbitmqPassword"]!);
                //});
                //configurator.ConfigureEndpoints(context);
                configurator.UseDelayedMessageScheduler();
                //configurator.ReceiveEndpoint("productupdate-type-queue", e =>
                //{
                //    e.BindDeadLetterQueue("productupdate-dead-letter-queue");
                //});
            });
        });
        services.AddOptions<MassTransitHostOptions>()
                          .Configure(options =>
                          {
                              options.WaitUntilStarted = true;
                              options.StartTimeout = TimeSpan.FromMinutes(1);
                              options.StopTimeout = TimeSpan.FromMinutes(1);
                          });
        return services;
    }
}