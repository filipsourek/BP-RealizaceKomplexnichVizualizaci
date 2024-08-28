using System.ComponentModel;

namespace SmartBuildingVisualization.Shared.Models
{
    public enum TimeFormat
    {
        [Description("Minutu")] Minute = 0, [Description("Hodinu")] Hour = 1, [Description("Den")] Day = 2, [Description("Měsíc")] Month = 3, [Description("Rok")] Year = 4
    }

    [Serializable]
    public class Building : IObject
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public List<Floor> Floors { get; set; } = [];
        public bool AllowCheck { get; set; } = true;

        public Building(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddFloor(int floorId, string name)
        {
            Floors.Add(new Floor(floorId, name));
        }

        public IEnumerable<Device> GetAllDevices()
        {
            return Floors.SelectMany(x => x.GetAllDevices()).ToList();
        }

        public Building DeepCopy()
        {
            Building othercopy = (Building)this.MemberwiseClone();
            othercopy.Floors = new List<Floor>();
            foreach (var floor in this.Floors)
            {
                othercopy.Floors.Add(floor.DeepCopy());
            }
            return othercopy;
        }
    }
}
