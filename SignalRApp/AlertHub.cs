using Microsoft.AspNetCore.SignalR;

namespace SignalRApp
{
    public class AlertHub : Hub
    {
        public async Task SendAlert(string alert)
        {
            await Clients.Others.SendAsync("ReceiveAlert", alert);
        }
    }
}
