using Blazored.LocalStorage;
using SmartBuildingVisualization.Client.Core.Interfaces;

namespace SmartBuildingVisualization.Client.Services
{
    public class DataStorageService: IDataStorage
    {
        private readonly ILocalStorageService _localStorage;
        public DataStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveData<T>(string key, T data)
        {
            await _localStorage.SetItemAsync(key, data);
        }

        public async Task<T> GetData<T>(string key)
        {

            if (!await _localStorage.ContainKeyAsync(key))
            {
                throw new KeyNotFoundException($"No data found for key: {key}.");
            }
            var data = await _localStorage.GetItemAsync<T>(key);

            return data;
        }
        public async Task RemoveData(string key)
        {
            await _localStorage.RemoveItemAsync(key);
        }
    }


    class DynamicStorage
    {
        public int GraphId { get; set; }
        public string Type { get; set; }
        public List<int> Ids { get; set; }
    }
}
