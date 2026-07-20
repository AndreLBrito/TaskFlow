using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskFlow.Api.Filters;
using TaskFlow.Application.DependencyInjection;
using TaskFlow.Infrastructure.DependencyInjection;
using TaskFlow.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/taskflow-api-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<GlobalExceptionFilter>();
    });

    builder.Services.AddOpenApi();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Angular", policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:4200",
                    "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "A connection string 'DefaultConnection' não foi configurada.");

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseCors("Angular");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(
        exception,
        "A API foi finalizada inesperadamente.");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
}
