using Microsoft.AspNetCore.Components;
using SmartBuildingVisualization.Client.Core.Interfaces;
using SmartBuildingVisualization.Client.Services;
using SmartBuildingVisualization.Shared.Models;


namespace SmartBuildingVisualization.Client.Pages.Components.Base
{
    public class GraphBase : ComponentBase
    {
        [Parameter] public List<IObject> SelectedObjects { get; set; } = new List<IObject>();
        [Parameter] public int Id { get; set; }

        [CascadingParameter(Name = "storeData")] public bool newUpdate { get; set; }
        public bool oldUpdate = false;


        public List<int> DeviceIds { get; set; } = new List<int>();

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeFormat TimeFormat { get; set; }

        [Inject]
        public IDataStorage DataStorage { get; set; }


        protected async Task StoreData()
        {
            if(DateStart == DateTime.MinValue)
            {
                await DataStorage.SaveData(Id.ToString(), new List<object> { (int)TimeFormat, null, null });
            }
            else
            {
                await DataStorage.SaveData(Id.ToString(), new List<object> { (int)TimeFormat, DateStart, DateEnd });
            }
        }

        protected override Task OnParametersSetAsync()
        {
            
            foreach (var obj in SelectedObjects)
            {
                foreach (var device in obj.GetAllDevices())
                {
                    if (!DeviceIds.Contains(device.Id))
                    {
                        DeviceIds.Add(device.Id);
                    }
                }         
            }

            return base.OnParametersSetAsync();
        }
    }
}
