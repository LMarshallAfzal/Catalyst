using System.Net;
using Microsoft.Extensions.Options;

using Catalyst.Models;
using Catalyst.Storage;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;

/// <summary>
/// Reads server port configuration from appsettings, allowing for default values and error handling.
/// </summary>
/// <param name="config">The application's configuration object.</param>
/// <param name="logger">A logger instance for recording informational messages.</param>
/// <returns> A tuple containing the configured host address and port number.</returns>
(string hostAddress, int port) ReadServerConfiguration(IConfiguration config, ILogger logger)
{
    string rawPort;
    string hostAddress;

    try 
    {
        rawPort = config.GetValue<string>("ServerSettings:Port") ?? "8080";
        hostAddress = config.GetValue<string>("ServerSettings:HostAddress") ?? "localhost";
    }
    catch (Exception e)
    {
        logger.LogError("Error reading configuration: {message}", e.Message);

        rawPort = "8080";
        hostAddress = "127.0.0.1";
    }

    bool isValidPort = int.TryParse(rawPort, out int port) && port >= 1 && port <= 65535;

    if (!isValidPort) 
    {
        logger.LogError("Error: Invalid port in configuration (must be a number between 1-65535)");
    }

    bool isValidHost = hostAddress == "127.0.0.1";

    if (!isValidHost)
    {
        logger.LogError("Error: Invalid host in configuration (must be in the format of an IPv4 address)");
    }

    return (hostAddress, port);
}

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Configuration");

// Configure Kestrel
var (hostAddress, port) = ReadServerConfiguration(config, logger);
builder.WebHost.ConfigureKestrel(options => 
{
    /// <summary>
    /// Configures Kestrel to listen on the specified IP address and port.
    /// <summary>
    /// <param name="hostAddress">The IP address to listen on.</param>
    /// <param name="port">The port number to listen on.</param>
    options.Listen(IPAddress.Parse(hostAddress), port);
});

builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("FileStorage"));
builder.Services.AddControllers();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

logger.LogInformation("Server configured to listen on: {hostAddress}:{port}", hostAddress, port);

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "v1/{controller=Files}/{action=Upload}/{id?}"
);

app.MapGet("/", () => "Hello from Minimal Test!");

app.Run();