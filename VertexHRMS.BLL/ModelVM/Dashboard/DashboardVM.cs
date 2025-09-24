namespace VertexHRMS.BLL.ModelVM.Dashboard
{
    public class DashboardVM
    {
        // KPIs
        public int Employees { get; set; }
        public int ActivePositions { get; set; }
        public int OpenJobReqs { get; set; }
        public decimal PayrollThisMonth { get; set; }
        public decimal PayrollDeltaPercent { get; set; }

        // Headcount series (labels aligned with values)
        public List<string> HeadcountLabels { get; set; } = new();
        public List<int> HeadcountValues { get; set; } = new();

        // Payroll series
        public List<string> PayrollLabels { get; set; } = new();
        public List<decimal> PayrollValues { get; set; } = new();

        // Dept distribution
        public List<string> DeptNames { get; set; } = new();
        public List<int> DeptValues { get; set; } = new();

        // Recent hires
        public List<string> RecentHireNames { get; set; } = new();
        public List<string> RecentHireDepts { get; set; } = new();
        public List<DateTime> RecentHireDates { get; set; } = new();

        // Recruitment KPIs
        public int Applicants { get; set; }
        public int Interviews { get; set; }
        public int Offers { get; set; }

        // Projects / revenue
        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> RevenueValues { get; set; } = new();
        public List<string> TopProjectNames { get; set; } = new();
        public List<decimal> TopProjectBudgets { get; set; } = new();

        // Project utilization (name -> percent)
        public List<string> ProjectUtilNames { get; set; } = new();
        public List<decimal> ProjectUtilPercent { get; set; } = new();

        // Trainings summary
        public int TrainingsTotal { get; set; }
        public int TrainingsCompleted { get; set; }
        public int TrainingsInProgress { get; set; }

        // Attendance summary (simple)
        public int AttendanceRecordsThisMonth { get; set; }
        public decimal AverageWorkMinutes { get; set; }

        // Flags / notes
        public bool IsDataLimited { get; set; } = false;
        public string DataNote { get; set; } = "";
    }
}
