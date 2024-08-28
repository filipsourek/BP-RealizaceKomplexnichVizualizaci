using Microsoft.AspNetCore.Components;
using SmartBuildingVisualization.Client.Core.Interfaces;
using SmartBuildingVisualization.Client.Core.Services;
using SmartBuildingVisualization.Shared.Models;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Web;

namespace SmartBuildingVisualization.Client.Services
{
    public class ApiService : IDataApiClient
    {
        private readonly HttpClient _client;
        private readonly CacheService _cacheService;
        private readonly NotificationsService _notificationsService;

        private readonly string _apiPath = "api/ConsumptionData/";

        public ApiService(HttpClient client, CacheService cacheService, NotificationsService notificationsService)
        {
            _client = client;
            _client.DefaultRequestHeaders.ConnectionClose = true;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Získá strukturu budovy z API
        /// </summary>
        /// <returns>Seznam objektů Building, představující strukturu budovy</returns>
        public async Task<List<Building>> GetStructure()
        {
            string methodString = "GetStructure";
            try
            {
                var cache = _cacheService.Get<List<Building>>(methodString);
                return cache;
            }
            catch (KeyNotFoundException e)
            {
                HttpResponseMessage response = await _client.GetAsync(_apiPath + methodString);
                if (response.IsSuccessStatusCode)
                {
                    var buildings = await response.Content.ReadFromJsonAsync<List<Building>>();
                    _cacheService.Set(methodString, buildings);
                    return buildings;
                }
                else
                {
                    return new List<Building>();
                }
            }      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate">Počáteční datum rozsahu.</param>
        /// <param name="endDate">Koncový datum rozsahu.</param>
        /// <param name="timeFormat">Formát času, ve kterém budou data vrácena.</param>
        /// <param name="ids">Volitelný seznam ID zařízení, pro která se mají získat data.</param>
        /// <returns>Seznam objektů Device se seskupenými daty.</returns>
        public async Task<List<Device>> GetDevicesData( DateTime startDate, 
                                                        DateTime endDate, 
                                                        TimeFormat timeFormat, 
                                                        List<int> ids = null)
        {
            string methodString = "GetDevicesDataInRange";
            var queryString = ConstructQueryString(startDate, endDate, timeFormat, ids);
            var url = $"{_apiPath}{methodString}{queryString}";

            try
            {
                var cache = _cacheService.Get<List<Device>>($"{methodString}{queryString}");
                return cache;
            }
            catch (KeyNotFoundException)
            {
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var devices = await response.Content.ReadFromJsonAsync<List<Device>>();

                    _cacheService.Set($"{methodString}{queryString}", devices);

                    return devices;
                }
                else
                {
                    return new List<Device>();
                }
            }
        }

        /// <summary>
        /// Získá sečtená data zařízení v daném časovém rozmezí.
        /// </summary>
        /// <param name="startDate">Počáteční datum rozsahu.</param>
        /// <param name="endDate">Koncový datum rozsahu.</param>
        /// <param name="ids">Volitelný seznam ID zařízení, pro která se mají získat data.</param>
        /// <returns>Seznam objektů Device s celkovými spotřebami.</returns>
        public async Task<List<Device>> GetSumDevicesData(DateTime startDate, DateTime endDate, List<int> ids = null)
        {
            string methodString = "GetSumDevicesInRange";
            var queryString = ConstructQueryString(startDate, endDate, TimeFormat.Day, ids);
            var url = $"{_apiPath}{methodString}{queryString}";

            try
            {
                var cache = _cacheService.Get<List<Device>>($"{methodString}{queryString}");
                return cache;
            }
            catch(KeyNotFoundException)
            {
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var devices = await response.Content.ReadFromJsonAsync<List<Device>>();
                    _cacheService.Set($"{methodString}{queryString}", devices);
                    return devices;
                }
                else
                {
                    return new List<Device>();
                }
            }
        }

        /// <summary>
        /// Získá nejnovější datum dat pro zadaná zařízení.
        /// </summary>
        /// <param name="ids">Seznam ID zařízení, pro která se má získat nejnovější datum.</param>
        /// <returns>Nejnovější datum dat pro zadaná zařízení.</returns>
        public async Task<DateTime> GetNewestDate(List<int> ids)
        {
            string idsQueryPart = string.Join("&", ids.Select(id => $"ids={id}"));
            var url = $"{_apiPath}GetNewestDate?{idsQueryPart}";

            HttpResponseMessage response = await _client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DateTime>();
            }
            else
            {
                return new DateTime(2000, 1, 1);
            }
        }


        /// <summary>
        /// Sestaví dotazovací řetězec pro HTTP GET požadavek.
        /// </summary>
        /// <param name="startDate">Počáteční datum rozsahu.</param>
        /// <param name="endDate">Koncový datum rozsahu.</param>
        /// <param name="timeFormat">Volitelný časový formát pro formátování dat.</param>
        /// <param name="ids">Volitelný seznam ID zařízení.</param>
        /// <returns>Vrací dotazovací řetězec s vytvořenými parametry.</returns>
        private static string ConstructQueryString(DateTime startDate, DateTime endDate, TimeFormat? timeFormat = null, List<int> ids = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query["startDate"] = startDate.ToString("o");
            query["endDate"] = endDate.ToString("o");

            if (timeFormat != null)
            {
                query["timeFormat"] = timeFormat.ToString();
            }

            if (ids != null && ids.Any())
            {
                foreach (var id in ids)
                {
                    query.Add("ids", id.ToString());
                }
            }

            var queryString = query.ToString();
            return "?" + queryString;
        }
    }
}
