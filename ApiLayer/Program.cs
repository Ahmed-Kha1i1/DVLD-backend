using DVLDApi;

var builder = WebApplication.CreateBuilder(args);

var app = builder.ConfigureServices().ConfigurePipline();

app.Run();
