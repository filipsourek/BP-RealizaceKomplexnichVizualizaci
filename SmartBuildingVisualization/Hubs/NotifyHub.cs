using Microsoft.AspNetCore.SignalR;


namespace SmartBuildingVisualization.Hubs
{
    public class NotifyHub: Hub
    {
        private readonly IHubContext<NotifyHub> _hubContext;

        public NotifyHub(IHubContext<NotifyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyClientsToRefreshCache()
        {
            await _hubContext.Clients.All.SendAsync("RefreshCache");
        }
    }
}
