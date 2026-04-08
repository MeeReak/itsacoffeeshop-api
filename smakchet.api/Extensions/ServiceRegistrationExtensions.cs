using smakchet.api.Filter;
using smakchet.application.DTOs.Category;
using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.DTOs.Ice;
using smakchet.application.DTOs.IFileStorageService;
using smakchet.application.DTOs.Order;
using smakchet.application.DTOs.Payment;
using smakchet.application.DTOs.Product;
using smakchet.application.DTOs.Size;
using smakchet.application.DTOs.Sugar;
using smakchet.application.DTOs.User;
using smakchet.application.DTOs.Variation;
using smakchet.application.Helpers;
using smakchet.application.Interfaces;
using smakchet.application.Interfaces.ICategory;
using smakchet.application.Interfaces.ICoffeeLevel;
using smakchet.application.Interfaces.IIce;
using smakchet.application.Interfaces.ILookupService;
using smakchet.application.Interfaces.IOrder;
using smakchet.application.Interfaces.IOrderItem;
using smakchet.application.Interfaces.IPayment;
using smakchet.application.Interfaces.IProduct;
using smakchet.application.Interfaces.ISize;
using smakchet.application.Interfaces.ISugar;
using smakchet.application.Interfaces.IUser;
using smakchet.application.Interfaces.IVariation;
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

        services.AddScoped<IMapper<Order, OrderReadDto, OrderDto, OrderUpdateDto>, OrderMapper>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<IMapper<Payment, PaymentReadDto, PaymentDto, PaymentUpdateDto>, PaymentMapper>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<ISugarRepository, SugarRepository>();

        services.AddScoped<IIceRepository, IceRepository>();

        services.AddScoped<ISizeRepository, SizeRepository>();

        services.AddScoped<ICoffeeLevelRepository, CoffeeLevelRepository>();

        services.AddScoped<IVariationRepository, VariationRepository>();

        services.AddScoped<ILookupService, LookupService>();

        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IBackgroundQueueService<PaymentStatusJob>,
            BackgroundQueueService<PaymentStatusJob>>();

        services.AddHostedService<PaymentStatusWorker>();


        services.AddScoped<ApiDeprecateActionFilter>();
        return services;
    }
}