
namespace VertexHRMS.DAL.Entities
{
    public class Department
    {
        public Department()
        {

        }
        public int DepartmentId { get; private set; }
        public string DepartmentName { get; private set; }
        public int? ParentDepartmentId { get; private set; }
        public Department ParentDepartment { get; private set; }
        public ICollection<Department> Children { get; private set; } = new List<Department>();
        public ICollection<Employee> Employees { get; private set; } = new List<Employee>();
        public ICollection<JobOpening> JobOpenings { get; private set; } = new List<JobOpening>();
    }
}
