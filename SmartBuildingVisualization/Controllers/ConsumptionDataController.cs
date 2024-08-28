using Microsoft.AspNetCore.Mvc;
using SmartBuildingVisualization.Shared.Models;
using SmartBuildingVisualization.Services;
using SmartBuildingVisualization.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace SmartBuildingVisualization.Controllers
{
    [Route("api/ConsumptionData/")]
    [ApiController]
    public class ConsumptionDataController : ControllerBase
    {
        StorageService _storageService;
        private readonly IHubContext<NotifyHub> _notifyHubContext;

        public ConsumptionDataController(StorageService storageService, IHubContext<NotifyHub> notifyHubContext)
        {
            _storageService = storageService;
            _notifyHubContext = notifyHubContext;

        }

        [HttpGet("GetDataInRange")]
        public async Task<ActionResult<List<ConsumptionData>>> GetConsumptionDataInRangeAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            throw new NotImplementedException();
        }

        [HttpGet("GetStructure")]
        public async Task<List<Building>> GetStructureAsync()
        {
            return await _storageService.GetStructureAsync();
        }


        [HttpGet("GetDevicesDataInRange")]
        public async Task<ActionResult<List<Device>>> GetConsumptionDataInRangeAsync(
                        [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
                        [FromQuery] TimeFormat timeFormat, [FromQuery] List<int> ids = null)
        {
            return await _storageService.GetDevicesDataAsync(startDate, endDate, timeFormat, ids);
        }

        [HttpGet("GetSumDevicesInRange")]
        public async Task<List<Device>> GetSumDevicesDataAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] List<int> ids = null)
        {
            await _notifyHubContext.Clients.All.SendAsync("RefreshCache");

            return await _storageService.GetSumDevicesDataAsync(startDate, endDate, ids);
        }


        [HttpGet("GetNewestDate")]
        public async Task<ActionResult<DateTime>> GetNewestDateAsync([FromQuery] List<int> ids)
        {
            return await _storageService.GetNewestDateAsync(ids);
        }

    }
}