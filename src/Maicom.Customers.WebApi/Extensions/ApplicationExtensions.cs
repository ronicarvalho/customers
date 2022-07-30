using System.Data.Common;
using Npgsql;

namespace Maicom.Customers.WebApi.Extensions;

public static class ApplicationExtensions
{
    public static void AddPostgreDatabase(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSingleton<DbConnection>(_ => new NpgsqlConnection(configuration.GetConnectionString("Postgres")));

    public static T AsValue<T>(this DbDataReader reader, int ordinal)
    {
        var value = reader.GetValue(ordinal);
        return (T)Convert.ChangeType(value, typeof(T));
    }
}