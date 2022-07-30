using System.Collections.ObjectModel;
using System.Data.Common;
using Maicom.Customers.WebApi.Extensions;
using Maicom.Customers.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Maicom.Customers.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MaicomCustomersController : ControllerBase
{
    private readonly DbConnection _connection;
    private readonly ILogger<MaicomCustomersController> _logger;

    public MaicomCustomersController(ILogger<MaicomCustomersController> logger, DbConnection connection)
    {
        _logger = logger;
        _connection = connection;
    }

    [HttpGet(Name = "maicom-customers")]
    public async Task<IReadOnlyCollection<Customer>> GetCustomers(CancellationToken cancellationToken)
    {
        var collection = new Collection<Customer>();

        await _connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = _connection.CreateCommand();

        command.CommandText = Statements.Customers;
        command.CommandTimeout = 120;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        if (!reader.HasRows) return collection;

        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            collection.Add(Customer.Create(
                reader.AsValue<ulong>(0),
                reader.AsValue<string>(1),
                reader.AsValue<string>(2),
                reader.AsValue<string>(3)
            ));

        await _connection.CloseAsync().ConfigureAwait(false);
        return new ReadOnlyCollection<Customer>(collection);
    }
}