using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RssGenerator;
using RssGenerator.Middlewares;
using RssGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json", true);
configurationBuilder.AddJsonFile("appsettings.Development.json", true);
configurationBuilder.AddJsonFile("appsettings.Release.json", true);
configurationBuilder.AddEnvironmentVariables();
IConfigurationRoot configuration = configurationBuilder.Build();

// Register the configuration as a service
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddScoped<IRssGenerator, RssGenerator.Services.RssGenerator>();

builder.Services.AddLogging(c => c.AddConsole());

var connectionString = configuration.GetConnectionString("RssGenerator");
builder.Services.AddDbContext<RssGeneratorContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddHttpClient();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleWare>();

app.Run();
