using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("EshopServer", username, password).WithPgAdmin().WithDataVolume();
var EshopDb = builder.ExecutionContext.IsRunMode ? postgres.AddDatabase("EshopDb") : builder.AddManagedResource("EshopDb");
var keyclock = builder.AddKeycloak("identity");
var redis = builder.ExecutionContext.IsRunMode ? builder.AddRedis("redis").WithRedisInsight() : builder.AddManagedResource("redis");
var rabbitMqUserName = builder.AddParameter("rabbitUserName", secret: true);
var rabbitmqPassword = builder.AddParameter("rabbitPassword", secret: true);
var rabbitMq = builder.ExecutionContext.IsRunMode ? builder.AddRabbitMQ("messageBroker", rabbitMqUserName, rabbitmqPassword,5672).WithManagementPlugin().WithImage("masstransit/rabbitmq") : builder.AddManagedResource("messageBroker");
builder.AddProject<Projects.Api>("api").WithReference(EshopDb)
    .WithReference(redis)
    .WithReference(rabbitMq)
    .WithReference(keyclock)
    .WaitFor(rabbitMq)
    .WaitFor(keyclock)
    .WaitFor(EshopDb)
    .WaitFor(redis)
    .WithEnvironment("rabbitUserName",rabbitMqUserName.Resource.Value)
    .WithEnvironment("rabbitmqPassword", rabbitmqPassword.Resource.Value);

builder.Build().Run();
