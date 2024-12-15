using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TravelRouteAPI;
using TravelRouteAPI.Shared;
using TravelRouteAPI.Shared.Enums;

var builder = WebApplication.CreateBuilder(args);
AppInfo.InitializeApp("TravelRouteAPI", builder.Environment.EnvironmentName);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
.CreateLogger();

Log.Logger = logger;
builder.Logging.AddSerilog(logger);

logger.Information($"Current host: {AppInfo.Current.Host}");
logger.Information($"Current environment: {AppInfo.Current.Environment}");
logger.Information($"Current app name: {AppInfo.Current.AppName}");
logger.Information($"Current app id: {AppInfo.Current.AppId}");

builder.Services.AddFusionCache();

DependencyInjection.Inject(builder.Services, builder.Configuration);

// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включаем Swagger UI
if (AppInfo.Current.Environment == AppEnvironment.Development)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();