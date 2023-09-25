using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Configuração da aplicação
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtém a instância de IConfiguration
var configuration = builder.Configuration;

builder.Services.AddHealthChecks()
    .AddSqlServer(
        connectionString: configuration.GetConnectionString("DefaultConnection"),
        healthQuery: "SELECT 1;",
        name: "Database",
        failureStatus: HealthStatus.Unhealthy
    );

builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("WeatherForecast", configuration.GetSection("HealthCheckUrl").Value);
})
.AddInMemoryStorage();

// Aqui você pode acessar a configuração diretamente se necessário
var applicationInsightsConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
builder.Services.AddApplicationInsightsTelemetry(applicationInsightsConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting()
            .UseEndpoints(config =>
            {
                config.MapHealthChecks("/healthcheck", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                config.MapHealthChecksUI(options => options.UIPath = "/dashboard");
            });

app.Run();
