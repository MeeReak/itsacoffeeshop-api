using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using smakchet.api.Extensions;
using smakchet.application.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomApiVersioning();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponse.ProduceErrorResponse;
});

var MyAllowSpecificOrigins = "_MyAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:3000",    // frontend origin
                    "https://mytrustedwebsite.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerConfiguration(provider);

}
app.UseCors(MyAllowSpecificOrigins);
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();

app.Run();
