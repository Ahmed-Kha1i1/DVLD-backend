using Azure.Identity;
using DVLD.API;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/MyAppLog.txt")
    .CreateLogger();

builder.Configuration.AddJsonFile("config.json");

//Configure Azure key vault
builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration["AzureKeyVault:VaultURI"]!),
    new ClientSecretCredential(
        tenantId: builder.Configuration["AzureKeyVault:TenantId"]!,
        clientId: builder.Configuration["AzureKeyVault:ClientId"]!,
        clientSecret: builder.Configuration["AzureKeyVault:ClientSecret"]!
        ));


builder.Host.UseSerilog();

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.Run();


