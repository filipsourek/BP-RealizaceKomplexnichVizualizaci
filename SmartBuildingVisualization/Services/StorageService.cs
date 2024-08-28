using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SmartBuildingVisualization.Hubs;
using SmartBuildingVisualization.Shared.Models;
using System.Diagnostics;

namespace SmartBuildingVisualization.Services
{
    public class StorageService
    {
        private List<Building> _buildings;
        private readonly string _filePath = "Data/data.json";
        
        private readonly IHubContext<NotifyHub> _notifyHubContext;
        private readonly NotifyHub _notifyHub;

        public StorageService(IHubContext<NotifyHub> notifyHubContext)
        {
            _notifyHubContext = notifyHubContext;
            _notifyHub = new NotifyHub(_notifyHubContext);
        }

        private async Task EnsureBuildingsLoadedAsync()
        {
            if (_buildings == null)
            {
                _buildings = await ReadFromJsonAsync(_filePath);
            }
        }

        private Task<List<Building>> ReadFromJsonAsync(string filePath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            return Task.Run(async () =>
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        using FileStream stream = File.OpenRead(filePath);
                        var buildings = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Building>>(stream) ?? new List<Building>();
                        sw.Stop();
                        return buildings;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while reading from JSON: {ex.Message}");
                }

                return new List<Building>();
            });
        }

        public async Task<List<Building>> GetStructureAsync()
        {
            await EnsureBuildingsLoadedAsync();

            return _buildings
                .Select(building =>
                {
                    var copy = building.DeepCopy();
                    copy.GetAllDevices().ForEach(device => device.Consumption.Clear());
                    return copy;
                })
                .ToList();
        }

        public async Task<List<Device>> GetDevicesDataAsync(DateTime dateStart, DateTime dateEnd, TimeFormat timeFormat, List<int> ids = null)
        {
            await EnsureBuildingsLoadedAsync();

            return _buildings
                .SelectMany(x => x.GetAllDevices())
                .Where(x => ids == null || ids.Contains(x.Id))
                .Select(x =>
                {
                    var copy = x.DeepCopy();
                    copy.Consumption = copy.GroupConsumption(dateStart, dateEnd, timeFormat);
                    return copy;
                })
                .ToList();
        }

        public async Task<List<Device>> GetSumDevicesDataAsync(DateTime dateStart, DateTime dateEnd, List<int> ids = null)
        {
            await EnsureBuildingsLoadedAsync();

            return _buildings
                .SelectMany(x => x.GetAllDevices())
                .Where(x => ids == null || ids.Contains(x.Id))
                .Select(x =>
                {
                    var copy = x.DeepCopy();
                    copy.Consumption = copy.SumConsumption(dateStart, dateEnd);
                    return copy;
                })
                .ToList();
        }

        public async Task<DateTime> GetNewestDateAsync(List<int> ids)
        {
            await EnsureBuildingsLoadedAsync();

            var consumptions = _buildings
                .SelectMany(x => x.GetAllDevices())
                .Where(x => ids.Contains(x.Id))
                .SelectMany(device => device.Consumption)
                .ToList();

            return consumptions.Any()
                ? consumptions.Max(c => c?.Date ?? DateTime.MinValue)
                : DateTime.MinValue;
        }

        public List<Building> ReadFromJson(string filePath)
        {
            List<Building> b = new List<Building>();

            if (File.Exists(filePath))
            {
                byte[] readJsonBytes = File.ReadAllBytes(filePath);
                b = System.Text.Json.JsonSerializer.Deserialize<List<Building>>(readJsonBytes);
            }
            return b;
        }

        public async Task UpdateData()
        {
            await _notifyHub.NotifyClientsToRefreshCache();
        }
    }
}