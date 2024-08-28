namespace SmartBuildingVisualization.Shared.Models
{
    public class Room : IObject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int DepartmentId { get; private set; }
        public List<Device> Devices { get; set; } = [];
        public bool AllowCheck { get; set; } = true;

        public Room(int id, string name, int departmentId)
		{
			Id = id;
			Name = name;
            DepartmentId = departmentId;
		}

		public void AddDevice(int deviceId, string name, int buildingId, int floorId, int roomId)
		{
			Devices.Add(new Device(deviceId, name, buildingId, floorId, roomId));
		}

        public IEnumerable<Device> GetAllDevices()
        {
            return Devices;
        }
        public Room DeepCopy()
        {
            Room othercopy = (Room)this.MemberwiseClone();
            othercopy.Devices = new List<Device>();
            foreach (var device in Devices)
            {
                othercopy.Devices.Add(device.DeepCopy());
            }
            return othercopy;
        }
    }
}
