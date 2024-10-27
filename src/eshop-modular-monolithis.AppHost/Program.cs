var builder = DistributedApplication.CreateBuilder(args);
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("EshopServer", username, password).WithPgAdmin().WithDataVolume();
var EshopDb = builder.ExecutionContext.IsRunMode ? postgres.AddDatabase("EshopDb") : builder.AddManagedResource("EshopDb");
builder.AddProject<Projects.Api>("api").WithReference(EshopDb);

builder.Build().Run();
