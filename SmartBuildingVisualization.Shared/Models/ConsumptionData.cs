namespace SmartBuildingVisualization.Shared.Models
{
    public class ConsumptionData
    {
        public int deviceId { get; set; }
        public DateTime Date { get; set; }
        public float Consumption { get; set; }
        public bool IsLine { get; set; } = false;
        
    }
}
