namespace EshopApp.Integration.Tests
{
    [TestClass]
    public class IntegrationTest1
    {
        // Instructions:
        // 1. Add a project reference to the target AppHost project, e.g.:
        //
        //    <ItemGroup>
        //        <ProjectReference Include="../MyAspireApp.AppHost/MyAspireApp.AppHost.csproj" />
        //    </ItemGroup>
        //
        // 2. Uncomment the following example test and update 'Projects.MyAspireApp_AppHost' to match your AppHost project:
        //
        [TestMethod]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.eshop_modular_monolithis_AppHost>();
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });
            await using var app = await appHost.BuildAsync();
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("api");
            await resourceNotificationService.WaitForResourceAsync("api", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
