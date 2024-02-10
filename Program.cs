using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Accessing Configuration
var config = builder.Configuration;
int port = config.GetValue<int>("ServerSettings:Port");
string hostAddress = config.GetValue<string>("ServerSettings:HostAddress") ?? "localhost";

// Configure Kestrel
builder.WebHost.ConfigureKestrel(options => {
    options.Listen(IPAddress.Parse(hostAddress), port);
});