var builder = DistributedApplication.CreateBuilder(args);
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("EshopServer", username, password).WithPgAdmin().WithDataVolume();
var EshopDb = builder.ExecutionContext.IsRunMode ? postgres.AddDatabase("EshopDb") : builder.AddManagedResource("EshopDb");
var keyclock = builder.AddKeycloak("identity");
var redis = builder.ExecutionContext.IsPublishMode ? builder.AddRedis("redis").WithRedisInsight() : builder.AddManagedResource("redis");
var rabbitMq = builder.ExecutionContext.IsPublishMode ? builder.AddRabbitMQ("messageBroker").WithManagementPlugin() : builder.AddManagedResource("messageBroker");
builder.AddProject<Projects.Api>("api").WithReference(EshopDb)
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithReference(keyclock)
    .WaitFor(rabbitMq)
    .WaitFor(keyclock)
    .WaitFor(EshopDb)
    .WaitFor(redis);

builder.Build().Run();
