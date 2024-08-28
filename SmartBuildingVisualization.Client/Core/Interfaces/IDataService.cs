using SmartBuildingVisualization.Shared.Models;
using System.Collections.ObjectModel;

namespace SmartBuildingVisualization.Client.Core.Interfaces
{
    public interface IDataService
    {
        public Task<ObservableCollection<Building>> GetStructureAsync();
        public Task<List<Device>> GetDevicesDataAsync(DateTime start, DateTime end, TimeFormat timeFormat, List<int> ids = null);
        public Task<List<Device>> GetSumDevicesDataAsync(DateTime start, DateTime end, List<int> ids = null);

        public Task<DateTime> GetNewestDateAsync(List<int> ids);
        public Task<List<DeviceConsumptionExtended>> GetConsumptionsExtendedAsync(DateTime start, DateTime end, TimeFormat timeFormat, List<int> ids = null);
        public Task<IObject> GetObjectFromId(int id);

        public ObservableCollection<Department> GetAllDepartments();



       

    }
}
