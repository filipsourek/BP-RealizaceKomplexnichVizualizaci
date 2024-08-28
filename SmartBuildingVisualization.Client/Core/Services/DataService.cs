using SmartBuildingVisualization.Shared.Models;
using System.Collections.ObjectModel;
using SmartBuildingVisualization.Client.Core.Interfaces;

namespace SmartBuildingVisualization.Client.Services
{

    public class DataService : IDataService
    {
        private ObservableCollection<Building> _buildings;
        private readonly ObservableCollection<Department> _departments;

        private readonly IDataApiClient _dataApiClient;


        public DataService(IDataApiClient dataApiClient)
        {
            _dataApiClient = dataApiClient;

            _departments = [new(91, "Oddělení A"), new(92, "Oddělení B"), new(93, "Oddělení C"), new(94, "Oddělení D")];            
        }


        public async Task<ObservableCollection<Building>> GetStructureAsync()
        {
            var temp = await _dataApiClient.GetStructure();

            return new ObservableCollection<Building>(temp);
        }

        public async Task<List<Device>> GetDevicesDataAsync( DateTime start, DateTime end, TimeFormat timeFormat, List<int> ids = null)
        {
            var temp = await _dataApiClient.GetDevicesData(start, end, timeFormat, ids);

            return temp;
        }

        public async Task<List<Device>> GetSumDevicesDataAsync(DateTime start, DateTime end, List<int> ids = null)
        {
            var temp = await _dataApiClient.GetSumDevicesData(start, end, ids);

            return temp;
        }

        public async Task<DateTime> GetNewestDateAsync(List<int> ids)
        {
            var temp = await _dataApiClient.GetNewestDate(ids);

            return temp;
        }

        public async Task<List<DeviceConsumptionExtended>> GetConsumptionsExtendedAsync(DateTime start, DateTime end, TimeFormat timeFormat, List<int> ids = null)
        {
            var temp = await _dataApiClient.GetDevicesData(start, end, timeFormat, ids);

            List<DeviceConsumptionExtended> consumptions = new List<DeviceConsumptionExtended>();

            foreach (var device in temp)
            {
                foreach (var consumption in device.Consumption)
                {
                    consumptions.Add(new DeviceConsumptionExtended(consumption.Date, consumption.Consumption, device.Id, device.BuildingId, device.FloorId, device.RoomId));
                }
            }

            return consumptions;
        }

        public async Task<IObject> GetObjectFromId(int id)
        {
            var buildings = await GetStructureAsync();
            return FindObjectById(buildings, id) ?? throw new KeyNotFoundException($"Object with ID {id} not found.");
        }

        private IObject FindObjectById(IEnumerable<IObject> objects, int id)
        {
            foreach (var obj in objects)
            {
                if (obj.Id == id)
                {
                    return obj;
                }

                if (obj is Building building)
                {
                    var result = FindObjectById(building.Floors, id);
                    if (result != null) return result;
                }
                else if (obj is Floor floor)
                {
                    var result = FindObjectById(floor.Rooms, id);
                    if (result != null) return result;
                }
                else if (obj is Room room)
                {
                    var result = FindObjectById(room.Devices, id);
                    if (result != null) return result;
                }
            }
            return null;
        }

        public ObservableCollection<Department> GetAllDepartments()
        {
            return _departments;
        }
    }
}
