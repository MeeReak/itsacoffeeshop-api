using Microsoft.EntityFrameworkCore;
using smakchet.dal.Models;

namespace smakchet.api.Extensions;

public static class DatabaseConnectionExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<SampleContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        return services;
    }
}