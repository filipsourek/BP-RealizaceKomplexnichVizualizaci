
namespace SmartBuildingVisualization.Shared.Models
{
    public class DeviceConsumption : IComparable<DeviceConsumption>
    {
        public int DeviceId { get; set; }
        public DateTime Date { get; set; }
        public float Consumption { get; set; }
        public bool IsLine { get; set; } = false;


        public DeviceConsumption(DateTime date, float consumption, int deviceId, bool isLine = false)
        {
            Date = date;
            Consumption = consumption;
            DeviceId = deviceId;
            IsLine = isLine;
        }

        public int CompareTo(DeviceConsumption other)
        {
            return Date.CompareTo(other.Date);
        }

        public DeviceConsumption DeepCopy()
        {
            DeviceConsumption othercopy = (DeviceConsumption)this.MemberwiseClone();
            return othercopy;
        }
    }

    public class DeviceConsumptionExtended : DeviceConsumption, IEquatable<DeviceConsumptionExtended>
    {
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public bool Visible { get; set; } = true;
        public DeviceConsumptionExtended(DateTime date, float consumption, int deviceId, int buildingId, int floorId, int roomId) : base(date, consumption, deviceId)
        {
            BuildingId = buildingId;
            FloorId = floorId;
            RoomId = roomId;
        }

        public bool Equals(DeviceConsumptionExtended other)
        {
            if (other is null)
                return false;

            return this.DeviceId == other.DeviceId
                && this.BuildingId == other.BuildingId
                && this.FloorId == other.FloorId
                && this.RoomId == other.RoomId;
        }

        public override bool Equals(object obj) => Equals(obj as DeviceConsumptionExtended);

        public override int GetHashCode()
        {
            return HashCode.Combine(DeviceId, BuildingId, FloorId, RoomId);
        }
    }
}
