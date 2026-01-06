using Asp.Versioning.ApiExplorer;


namespace smakchet.api.Extensions;

public static class SwaggerApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {

        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            var descriptions = provider.ApiVersionDescriptions;
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                opt.SwaggerEndpoint(url, name);
            }
        });
        return app;
    }
}