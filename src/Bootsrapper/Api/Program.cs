var builder = WebApplication.CreateBuilder(args);
// add services to the container
builder.Services
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder, builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

//configure http request pipeline
app.UseCatalogModule()
    .UseOrderingModule()
    .UseBasketModule();
app.MapGet("/", () =>
{
    Results.Ok();
});
app.Run();
