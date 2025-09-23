using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Entities
{
    public class EmployeeTraining
    {
        public int Id { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public string Title { get; private set; }
        public string Status { get; private set; }
        public DateTime? DueDate { get; private set; }
    }
}
