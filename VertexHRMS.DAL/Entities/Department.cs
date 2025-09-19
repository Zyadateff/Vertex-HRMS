namespace VertexHRMS.DAL.Entities
{
    public class Department
    {
        public Department()
        {
            Children = new List<Department>();
            Employees = new List<Employee>();
            JobOpenings = new List<JobOpening>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ParentDepartmentId { get; set; }
        public Department ParentDepartment { get; set; }
        public ICollection<Department> Children { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<JobOpening> JobOpenings { get; set; }

        public static Department Create(string departmentName, int? parentDepartmentId = null)
        {
            return new Department
            {
                DepartmentName = departmentName,
                ParentDepartmentId = parentDepartmentId
            };
        }
    }
}