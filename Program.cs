using System.Net;
using Catalyst.Models;
using Catalyst.Storage;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;
string rawPort;
string hostAddress;

try 
{
    rawPort = config.GetValue<string>("ServerSettings:Port") ?? "8080";
    hostAddress = config.GetValue<string>("ServerSettings:HostAddress") ?? "localhost";
}
catch (Exception e)
{
    Console.Error.WriteLine("Error reading configuration: " + e.Message);

    rawPort = "8080";
    hostAddress = "127.0.0.1";
}

bool isValidPort = int.TryParse(rawPort, out int port) && port >= 1 && port <= 65535;

if (!isValidPort) 
{
    Console.Error.WriteLine("Error: Invalid port in configuration (must be a number between 1-65535)");
}

bool isValidHost = hostAddress == "127.0.0.1";

if (!isValidHost)
{
    Console.Error.WriteLine("Error: Invalid host in configuration (must be in the format of an IPv4 address)");
}

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options => 
{
    options.Listen(IPAddress.Parse(hostAddress), port);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Configuration");
logger.LogInformation("Server configured to listen on: {hostAddress}:{port}", hostAddress, port);

var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "v1/{controller=Files}/{action=Upload}/{id?}"
);

app.MapGet("/", () => "Hello from Minimal Test!");

app.Run();