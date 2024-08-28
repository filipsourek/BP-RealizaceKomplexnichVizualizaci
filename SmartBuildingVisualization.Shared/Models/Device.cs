
namespace SmartBuildingVisualization.Shared.Models
{

    public class Device : IObject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<DeviceConsumption> Consumption { get; set; } = [];
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public bool AllowCheck { get; set; } = true;

        public Device(int id, string name, int buildingId, int floorId, int roomId)
		{
			Id = id;
			Name = name;
            BuildingId = buildingId;
            FloorId = floorId;
            RoomId = roomId;
        }

        public Device DeepCopy()
        {
            Device othercopy = (Device)this.MemberwiseClone();
            othercopy.Consumption = new List<DeviceConsumption>();
            foreach (var consumption in this.Consumption)
            {
                othercopy.Consumption.Add(consumption.DeepCopy());
            }
            return othercopy;
        }

            public void AddConsumption(DateTime hour, float consumption)
		{
            var consumptionToUpdate = Consumption.Find(x => x.Date == hour);
            if(consumptionToUpdate != null)
            {
                consumptionToUpdate.Consumption = consumption;
                return;
            }               
			Consumption.Add(new DeviceConsumption(hour, consumption, this.Id));
        }
        public void AddConsumption(List<DeviceConsumption> list)
        {
            Consumption.AddRange(list);
        }
        
        public List<DeviceConsumption> SumConsumption(DateTime start, DateTime end)
        {
            List<DeviceConsumption> consumptions = new List<DeviceConsumption>();
            consumptions.Add(new DeviceConsumption(start, Consumption.Where(x => x.Date >= start && x.Date <= end).Sum(x => x.Consumption), Id));
            return consumptions;
        }

        public List<DeviceConsumption> GroupConsumption(DateTime start, DateTime end, TimeFormat timeFormat)
        {
            return timeFormat switch
            {
                TimeFormat.Minute => Consumption.Where(x => x.Date >= start && x.Date <= end).ToList(),
                TimeFormat.Hour => GroupConsumptionByHour().Where(x => x.Date >= start && x.Date <= end).ToList(),
                TimeFormat.Day => GroupConsumptionByDay().Where(x => x.Date >= start && x.Date <= end).ToList(),
                TimeFormat.Month => GroupConsumptionByMonth().Where(x => x.Date >= start && x.Date <= end).ToList(),
                TimeFormat.Year => GroupConsumptionByYear().Where(x => x.Date >= start && x.Date <= end).ToList(),
                _ => GroupConsumptionByDay().Where(x => x.Date >= start && x.Date <= end).ToList(),
            };
        }

        /// <summary>
        /// Seskupí data podle hodin
        /// </summary>
        /// <returns>List spotřeb seskupených podle hodin.</returns>
        public List<DeviceConsumption> GroupConsumptionByHour()
        {
            return Consumption
                .GroupBy(dc => new DateTime(dc.Date.Year, dc.Date.Month, dc.Date.Day, dc.Date.Hour, 0, 0))
                .Select(group => new DeviceConsumption(group.Key, group.Sum(dc => dc.Consumption), this.Id))
                .ToList();
        }

        /// <summary>
        /// Seskupí data podle dní
        /// </summary>
        /// <returns>List spotřeb seskupených podle dní.</returns>
        public List<DeviceConsumption> GroupConsumptionByDay()
        {            
            return Consumption
                .GroupBy(dc => dc.Date.Date)
                .Select(group => new DeviceConsumption(group.Key, group.Sum(dc => dc.Consumption), this.Id))
                .ToList();

        }
        /// <summary>
        /// Seskupí data podle měsíců
        /// </summary>
        /// <returns>List spotřeb seskupených podle měsíců.</returns>
        public List<DeviceConsumption> GroupConsumptionByMonth()
		{
			return Consumption
                .GroupBy(dc => new DateTime(dc.Date.Year, dc.Date.Month, 1))
                .Select(group => new DeviceConsumption(group.Key, group.Sum(dc => dc.Consumption), this.Id))
                .ToList();
        }
        /// <summary>
        /// Seskupí data podle roků
        /// </summary>
        /// <returns>List spotřeb seskupených podle roků.</returns>
        public List<DeviceConsumption> GroupConsumptionByYear()
		{
            return Consumption
                .GroupBy(dc => new DateTime(dc.Date.Year, 1, 1))
                .Select(group => new DeviceConsumption(group.Key, group.Sum(dc => dc.Consumption), this.Id))
                .ToList();			 
		}      

        public IEnumerable<Device> GetAllDevices()
        {
            List<Device> devices = new();
            devices.Add(this);
            return devices;
        }
    }
}
