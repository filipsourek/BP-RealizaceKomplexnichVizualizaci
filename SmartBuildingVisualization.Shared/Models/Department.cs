namespace SmartBuildingVisualization.Shared.Models
{
    public class Department
	{
        public int DepartmentId { get; set; }
		public string Name { get; set; }

        public Department(int departmentId, string name)
        {
            DepartmentId = departmentId;
            Name = name;
        }
    }
}
