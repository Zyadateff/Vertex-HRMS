// DAL/Data/ApplicationDbContext.cs (Updated)
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Database { 
    public class VertexHRMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public VertexHRMSDbContext(DbContextOptions<VertexHRMSDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<OvertimeRequest> OvertimeRequests { get; set; }
        public DbSet<Resignation> Resignations { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LeaveEntitlement> LeaveEntitlements { get; set; }
        public DbSet<LeaveLedger> LeaveLedgers { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var fk in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.EmploymentType).HasMaxLength(20);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.ImagePath).HasMaxLength(255);

                // Unique constraints
                entity.HasIndex(e => e.EmployeeCode).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                // Relationships
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Position)
                      .WithMany(p => p.Employees)
                      .HasForeignKey(e => e.PositionID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Manager)
                      .WithMany(e => e.DirectReports)
                      .HasForeignKey(e => e.ManagerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.IdentityUser)
                      .WithOne(u => u.Employee)
                      .HasForeignKey<Employee>(e => e.IdentityUserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Department Configuration
            builder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.DepartmentId);
                entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);

                entity.HasOne(d => d.ParentDepartment)
                      .WithMany(d => d.Children)
                      .HasForeignKey(d => d.ParentDepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Position Configuration
            builder.Entity<Position>(entity =>
            {
                entity.HasKey(p => p.PositionId);
                entity.Property(p => p.PositionName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.BaseSalary).HasColumnType("decimal(18,2)");
            });

            // ApplicationUser Configuration
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed Data
            SeedInitialData(builder);
        }

        private static void SeedInitialData(ModelBuilder builder)
        {
            // Seed Departments
            builder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Human Resources" },
                new Department { DepartmentId = 2, DepartmentName = "Information Technology" },
                new Department { DepartmentId = 3, DepartmentName = "Finance" },
                new Department { DepartmentId = 4, DepartmentName = "Marketing" },
                new Department { DepartmentId = 5, DepartmentName = "Operations" }
            );

            // Seed Positions
            builder.Entity<Position>().HasData(
                new Position { PositionId = 1, PositionName = "Software Developer", BaseSalary = 75000 },
                new Position { PositionId = 2, PositionName = "HR Manager", BaseSalary = 65000 },
                new Position { PositionId = 3, PositionName = "Financial Analyst", BaseSalary = 60000 },
                new Position { PositionId = 4, PositionName = "Marketing Specialist", BaseSalary = 55000 },
                new Position { PositionId = 5, PositionName = "Operations Manager", BaseSalary = 70000 },
                new Position { PositionId = 6, PositionName = "System Administrator", BaseSalary = 68000 },
                new Position { PositionId = 7, PositionName = "Accountant", BaseSalary = 50000 },
                new Position { PositionId = 8, PositionName = "HR Assistant", BaseSalary = 40000 }
            );
        }
    }
}