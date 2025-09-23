using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Database
{
    public class VertexHRMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public VertexHRMSDbContext(DbContextOptions<VertexHRMSDbContext> options) : base(options)
        {
        }

        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<Deduction> Deductions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ExitClearance> ExitClearances { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayCalendar> HolidayCalendars { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<LeaveApproval> LeaveApprovals { get; set; }
        public DbSet<LeaveEntitlement> LeaveEntitlements { get; set; }
        public DbSet<LeaveLedger> LeaveLedgers { get; set; }
        public DbSet<LeavePolicy> LeavePolicies { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveRequestDay> LeaveRequestDays { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Onboarding> Onboardings { get; set; }
        public DbSet<OvertimeRequest> OvertimeRequests { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<PayrollDeduction> PayrollDeductions { get; set; }
        public DbSet<PayrollRun> PayrollRuns { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Resignation> Resignations { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        
        }
    }
}