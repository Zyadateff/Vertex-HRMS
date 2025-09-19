namespace VertexHRMS.DAL.Entities
{
    public class Position
    {
        public Position()
        {
            Employees = new List<Employee>();
            JobOpenings = new List<JobOpening>();
        }

        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public decimal BaseSalary { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<JobOpening> JobOpenings { get; set; }

        public static Position Create(string positionName, decimal baseSalary)
        {
            return new Position
            {
                PositionName = positionName,
                BaseSalary = baseSalary
            };
        }
    }
}