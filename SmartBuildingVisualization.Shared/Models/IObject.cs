namespace SmartBuildingVisualization.Shared.Models
{
    public interface IObject
    {
        int Id { get;  }
        string Name { get;  }

        IEnumerable<Device> GetAllDevices();

        bool AllowCheck { get; set; }
    }
}
