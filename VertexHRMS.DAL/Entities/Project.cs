using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.DAL.Enum;

namespace VertexHRMS.DAL.Entities
{
    public class Project
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public ProjectStatus Status { get; private set; }
        public decimal Budget { get; private set; }
        public ICollection<ProjectTask> Tasks { get; private set; }
    }
}
