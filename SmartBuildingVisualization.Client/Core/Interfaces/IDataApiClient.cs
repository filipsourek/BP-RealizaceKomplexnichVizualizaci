using SmartBuildingVisualization.Shared.Models;

namespace SmartBuildingVisualization.Client.Core.Interfaces
{
    public interface IDataApiClient
    {
        Task<List<Building>> GetStructure();
        Task<List<Device>> GetDevicesData(DateTime start, DateTime end, TimeFormat timeFormat, List<int> ids);
        Task<List<Device>> GetSumDevicesData(DateTime start, DateTime end, List<int> ids);
        Task<DateTime> GetNewestDate(List<int> ids);
    }
}
