using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using smakchet.dal.Models;

namespace smakchet.api.Extensions;

public static class DatabaseConnectionExtensions
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        var minioConfig = configuration.GetSection("Minio");
        var accessKey = minioConfig["AccessKey"];
        var secretKey = minioConfig["SecretKey"];
        var serviceUrl = minioConfig["ServiceUrl"];

        services.AddDbContext<SmakchetContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            };
            return new AmazonS3Client(accessKey, secretKey, config);
        });

        return services;
    }
}