using smakchet.application.Interfaces.IUser;
using smakchet.application.Mappings;
using smakchet.application.Repositories;
using smakchet.api.Filter;
using smakchet.application.Interfaces;
using smakchet.application.Services;

namespace smakchet.api.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        services.AddScoped<IUserMapper, UserMapper>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<ApiDeprecateActionFilter>();
        return services;
    }
}