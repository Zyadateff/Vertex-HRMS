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
        public DbSet<LeaveRequestEmail> LeaveRequestEmails { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<EmployeeTraining> EmployeeTrainings { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<GoogleFormApplication> GoogleFormApplications { get; set; }
        public DbSet<ATSCandidate> ATSCandidates { get; set; }
        public DbSet<CandidateReview> CandidateReviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Applicant -> ApplicationUser (AspNetUsers)
            modelBuilder.Entity<Applicant>()
                .HasOne(a => a.IdentityUser)
                .WithMany() // Applicant ãÔ ãÍÊÇÌíä äÑÈØå ÈÜ ICollection Ýí ApplicationUser
                .HasForeignKey(a => a.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> ApplicationUser (one-to-one)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.IdentityUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}