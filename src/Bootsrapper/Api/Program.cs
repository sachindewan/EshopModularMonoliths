using Carter;
using Shared.Exceptions.Handler;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);
// add services to the container
//common services: carter, mediatr, fluentvalidation, masstransit
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder
    .AddBasketModule(builder.Configuration)
    .AddCatalogModule(builder.Configuration);
    //.AddOrderingModule(builder.Configuration);
builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

builder.AddServiceDefaults();

var app = builder.Build();
app.MapCarter();
app.UseExceptionHandler(options => { });
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
