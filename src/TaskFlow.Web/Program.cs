using Serilog;
using TaskFlow.Application.DependencyInjection;
using TaskFlow.Infrastructure.DependencyInjection;
using TaskFlow.Web.Filters;
using TaskFlow.Web.Mapping;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/taskflow-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllersWithViews(options =>
    {
        options.Filters.Add<ValidationExceptionFilter>();
        options.Filters.Add<GlobalExceptionFilter>();
    });

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();

    MapsterConfiguration.RegisterMappings();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "A aplicação foi finalizada inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}