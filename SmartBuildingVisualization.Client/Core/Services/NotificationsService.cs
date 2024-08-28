using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

namespace SmartBuildingVisualization.Client.Core.Services
{
    public class NotificationsService
    {
        private readonly HubConnection _hubConnection;

        public NotificationsService(NavigationManager navigationManager, CacheService cacheService)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/notifyhub"))
                .Build();

            _hubConnection.On("RefreshCache", () =>
            {
                cacheService.ClearCache();
            });

            _hubConnection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}