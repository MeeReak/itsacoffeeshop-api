using smakchet.application.Interfaces.IUser;
using smakchet.application.Mappings;
using smakchet.application.Repositories;
using smakchet.api.Filter;

namespace smakchet.api.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<ApiDeprecateActionFilter>();
        return services;
    }
}