var builder = DistributedApplication.CreateBuilder(args);
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("EshopServer", username, password).WithPgAdmin();

var Eshop = postgres.AddDatabase("Eshop");
builder.AddProject<Projects.Api>("api").WithReference(postgres).WithReference(Eshop);

builder.Build().Run();
