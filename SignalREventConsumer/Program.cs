using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

var signalRUrl = configuration.GetSection("SignalRConfig")["SignalRUrl"];

var hubConnection = new HubConnectionBuilder()
           .WithUrl(signalRUrl)
           .WithAutomaticReconnect()
           .Build();

hubConnection.On<string>("ReceiveAlert", (message) =>
{
    Console.WriteLine($"Received message: {message}");
});


try
{
    await hubConnection.StartAsync();
    Console.WriteLine("Connected to SignalR hub.");
    await Task.Run(() => Thread.Sleep(Timeout.Infinite));
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while connecting to the SignalR hub: {ex.Message}");
}
finally
{
    await hubConnection.DisposeAsync();
}