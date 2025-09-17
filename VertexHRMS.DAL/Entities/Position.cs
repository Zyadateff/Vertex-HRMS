
namespace VertexHRMS.DAL.Entities
{
    public class Position
    {
        public Position()
        {

        }
        public Position(string positionName, decimal baseSalary)
        {
            PositionName = positionName;
            BaseSalary = baseSalary;
        }

        public int PositionId { get; private set; }
        public string PositionName { get; private set; }
        public decimal BaseSalary { get; private set; }
        public ICollection<Employee> Employees { get; private set; } = new List<Employee>();
        public ICollection<JobOpening> JobOpenings { get; private set; } = new List<JobOpening>();
    }
}
