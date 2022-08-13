namespace Maicom.Customers.WebApi;

public static class Program
{
    public static void Main() => Host
        .CreateDefaultBuilder()
        .ConfigureLogging(builder => builder.ClearProviders())
        .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
        .Build()
        .Run();
}