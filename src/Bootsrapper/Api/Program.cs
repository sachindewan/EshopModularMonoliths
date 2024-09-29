var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// add services to the container
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);
var app = builder.Build();

app.MapDefaultEndpoints();

//configure http request pipeline
app.UseCatalogModule()
    .UseOrderingModule()
    .UseBasketModule();

app.Run();
