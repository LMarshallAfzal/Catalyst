using System.Net;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;
int port = config.GetValue<int>("ServerSettings:Port");
string hostAddress = config.GetValue<string>("ServerSettings:HostAddress") ?? "localhost";

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options => {
    options.Listen(IPAddress.Parse(hostAddress), port);
});

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Configuration");
logger.LogInformation("Server configured to listen on: {hostAddress}:{port}", hostAddress, port);