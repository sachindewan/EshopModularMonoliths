

public static class DistributedApplicationBuilderExtensions
{
    public static IResourceBuilder<IResourceWithConnectionString> AddManagedResource(this IDistributedApplicationBuilder builder, string connectionString, string? environmentVariableName=null)
    {
        return builder.AddConnectionString(connectionString, environmentVariableName);
    }
}

