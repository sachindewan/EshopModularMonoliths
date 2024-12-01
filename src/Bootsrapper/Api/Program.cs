
using Basket.Basket.EventHandlers;
using Catalog.Products.Events;
using Catalog.Products.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;

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

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, basketAssembly, orderingAssembly);
builder.AddRedisClient("redis");
builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
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
app.UseAuthentication();
app.UseAuthorization();
//configure http request pipeline
app.UseCatalogModule()
    .UseOrderingModule()
    .UseBasketModule();
app.MapGet("/", async (IPublishEndpoint publish, CancellationToken token) =>
{
    Results.Ok();
});
app.Run();



