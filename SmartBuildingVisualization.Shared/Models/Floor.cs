namespace SmartBuildingVisualization.Shared.Models
{
    [Serializable]
    public class Floor : IObject
    {
		public int Id { get; private set; }
		public string Name { get; private set; }
		public List<Room> Rooms { get; set; } = [];
        public bool AllowCheck { get; set; } = true;

        public Floor(int id, string name) 
		{
			Id = id;
			Name = name;
		}

		public void AddRoom(int roomId, string name, int departmentId)
		{
			Rooms.Add(new Room(roomId, name, departmentId));
		}       

        public IEnumerable<Device> GetAllDevices()
        {
            return Rooms.SelectMany(x => x.GetAllDevices()).ToList();
        }

        public Floor DeepCopy()
        {
            Floor othercopy = (Floor)this.MemberwiseClone();
            othercopy.Rooms = new List<Room>();
            foreach (var floor in this.Rooms)
            {
                othercopy.Rooms.Add(floor.DeepCopy());
            }
            return othercopy;
        }

    }
}
