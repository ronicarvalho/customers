using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Maicom.Customers.WebApiTests.Models;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Maicom.Customers.WebApiTests.Services;

public class MaicomCustomersWebApiTests: IClassFixture<CustomersWebApiSetup>
{
    private readonly HttpClient _client;

    public MaicomCustomersWebApiTests(CustomersWebApiSetup setup)
    {
        _client = setup.CreateClient();
    }
    
    [Fact]
    public async Task WhenGetCustomersShouldReturnsCustomerList()
    {
        var cancellationToken = CancellationToken.None;

        var response = await _client
            .GetAsync("/api/v1/customer", cancellationToken)
            .ConfigureAwait(false);

        var content = await response.Content
            .ReadFromJsonAsync<CustomerTest[]>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
        content.Should().NotBeNull();
        content.Should().HaveCount(10);
    }
}