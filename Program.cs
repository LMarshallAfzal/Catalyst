using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;
int port;
string hostAddress;

try 
{
    port = config.GetValue<int>("ServerSettings:Port");
    hostAddress = config.GetValue<string>("ServerSettings:HostAddress") ?? "localhost";
}
catch (Exception e)
{
    Console.Error.WriteLine("Error reading configuration: " + e.Message);

    port = 8080;
    hostAddress = "127.0.0.1";
}

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options => {
    options.Listen(IPAddress.Parse(hostAddress), port);
});

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Configuration");
logger.LogInformation("Server configured to listen on: {hostAddress}:{port}", hostAddress, port);