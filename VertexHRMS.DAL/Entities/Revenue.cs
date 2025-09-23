using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Entities
{
    public class Revenue
    {
        public int Id { get; private set; }
        public DateTime MonthYear { get; private set; }
        public decimal Amount { get; private set; } 
        public decimal Expenses { get; private set; }
        public decimal Profit => Amount - Expenses; 
    }
}
