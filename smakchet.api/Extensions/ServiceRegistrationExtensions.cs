using smakchet.api.Filter;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.User;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ICategory;
using smakchet.application.Interfaces.IProduct;
using smakchet.application.Interfaces.IUser;
using smakchet.application.Mappings;
using smakchet.application.Repositories;
using smakchet.application.Services;
using smakchet.dal.Models;

namespace smakchet.api.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<IMapper<User, UserReadDto, UserDto, UserUpdateDto>, UserMapper>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IMapper<Category, CategoryReadDto, CategoryDto, CategoryUpdateDto>, CategoryMapper>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IMapper<Product, ProductReadDto, ProductDto, ProductUpdateDto>, ProductMapper>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<ApiDeprecateActionFilter>();
        return services;
    }
}