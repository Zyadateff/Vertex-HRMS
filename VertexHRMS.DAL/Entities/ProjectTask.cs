
using VertexHRMS.DAL.Enum;

namespace VertexHRMS.DAL.Entities
{
    public class ProjectTask
    {
        public int Id { get; private set; }
        public int ProjectId { get; private set; }
        public Project Project { get; private set; }
        public string Title { get; private set; }
        public ProjectStatus Status { get; private set; } 
        public int EstimatedHours { get; private set; }
        public DateTime StartDate { get; private set; }
        public int SpentHours { get; private set; }
        public DateTime? DueDate { get; private set; }
        public int AssignedToEmployeeId { get; private set; }
        public Employee AssignedTo { get; private set; }
    }
}
