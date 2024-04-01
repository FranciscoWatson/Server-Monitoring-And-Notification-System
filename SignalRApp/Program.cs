
using SignalRApp;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");


builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<AlertHub>("/alerthub");
});


app.Run();