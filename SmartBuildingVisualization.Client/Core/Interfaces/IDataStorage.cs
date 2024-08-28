namespace SmartBuildingVisualization.Client.Core.Interfaces
{
    public interface IDataStorage
    {
        public Task SaveData<T>(string key, T data);
        public Task<T> GetData<T>(string key);
        public Task RemoveData(string key);
    }
}
