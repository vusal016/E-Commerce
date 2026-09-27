var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});
builder.Services.AddOpenApi();
builder.Services.AddModuleRegistrations(builder.Configuration);
builder.Services.AddMessaging();
builder.Services.AddServices();
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddSingleton<ISaveChangesInterceptor, AuditInterceptor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.Services.MigrateAllDatabasesAsync();


app.UseMiddleware<GlobalExceptionHandler>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();