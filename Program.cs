using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;
var routes = config.GetSection("Routes").Get<IEnumerable<RouteDefinition>>();
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

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Configuration");
logger.LogInformation("Server configured to listen on: {hostAddress}:{port}", hostAddress, port);

var app = WebApplication.Create();

foreach (var route in routes)
{
    if (route.Method != null)
    {
        switch (route.Method.ToUpper())
        {
            case "GET":
                app.MapGet(route.Path, () => route.Response ?? "Route Matched!");
                break;
            case "POST":
                app.MapPost(route.Path, () => route.Response ?? "Route Matched!");
                break;
            default:
                Console.Error.WriteLine($"Unsupported HTTP Method: {route.Method}");
                break;
        }
    } else 
    {
        Console.Error.WriteLine("Error: No HTTP method specified (must have one of the following methods: GET, POST, PUT, PATCH, DELETE).");
    }
}

app.Run();