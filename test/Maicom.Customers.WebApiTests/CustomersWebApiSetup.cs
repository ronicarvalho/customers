using System.IO;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Maicom.Customers.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Maicom.Customers.WebApiTests;

public class CustomersWebApiSetup: WebApplicationFactory<Startup>, IAsyncLifetime
{
    private static readonly string ComposeFile = ConfigurationFile("docker-compose.yaml");
    private ICompositeService? _containerService;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
            config.AddJsonFile(ConfigurationFile("appsettings.test.json")));
    }

    public Task InitializeAsync()
    {
        _containerService = new Builder()
            .UseContainer()
            .FromComposeFile(ComposeFile)
            .ForceBuild()
            .RemoveOrphans()
            .Build()
            .Start();

        foreach (var container in _containerService.Containers)
            container.StopOnDispose = true;

        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        foreach (var container in _containerService.Containers)
            container?.Dispose();
        
        _containerService.Dispose();

        return Task.CompletedTask;
    }

    private static string ConfigurationFile(string file)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), file);
    }
}