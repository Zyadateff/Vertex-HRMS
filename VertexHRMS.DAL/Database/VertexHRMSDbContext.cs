using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Database
{
    public class VertexHRMSDbContext:DbContext
    {
        public VertexHRMSDbContext(DbContextOptions<VertexHRMSDbContext> options) : base(options)
        {
        }

    }
}
