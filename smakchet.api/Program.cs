using Asp.Versioning.ApiExplorer;
using smakchet.application.Middleware;
using smakchet.api.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomApiVersioning();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponse.ProduceErrorResponse;
});

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAppDbContext(builder.Configuration);
builder.Services.AddAppServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerConfiguration(provider);

}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
