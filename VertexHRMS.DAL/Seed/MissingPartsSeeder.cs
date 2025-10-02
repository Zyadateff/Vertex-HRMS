using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using VertexHRMS.DAL.Entities;
namespace VertexHRMS.DAL.Seed
{
    

    public static class MissingPartsSeeder
    {
        private static void SetPrivateProperty(object target, string propertyName, object value)
        {
            if (target == null) return;
            var t = target.GetType();
            var prop = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null) throw new InvalidOperationException($"Property '{propertyName}' not found on type {t.FullName}");
            var setMethod = prop.GetSetMethod(true);
            if (setMethod == null) throw new InvalidOperationException($"Property '{propertyName}' on {t.FullName} has no setter (even non-public).");
            setMethod.Invoke(target, new[] { value });
        }

        public static async Task SeedMissingAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;
            var db = provider.GetRequiredService<VertexHRMSDbContext>();
            var rnd = new Random();

            try
            {
                // 1) WorkSchedules
                if (!await db.WorkSchedules.AnyAsync(ws => ws.Name == "Morning" || ws.Name == "Evening"))
                {
                    db.WorkSchedules.AddRange(
                        new WorkSchedule("Morning", new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0)),
                        new WorkSchedule("Evening", new TimeSpan(15, 0, 0), new TimeSpan(22, 0, 0))
                    );
                    await db.SaveChangesAsync();
                    Console.WriteLine("Seeded WorkSchedules");
                }

                // 2) Projects (create one if none)
                Project mainProject = null;
                if (!await db.Projects.AnyAsync())
                {
                    var proj = Activator.CreateInstance(typeof(Project));
                    SetPrivateProperty(proj, nameof(Project.Name), "Internal HR System");
                    SetPrivateProperty(proj, nameof(Project.StartDate), DateTime.Now.AddMonths(-3));
                    // set enum - ensure using correct enum type name
                    SetPrivateProperty(proj, nameof(Project.Status), VertexHRMS.DAL.Enum.ProjectStatus.Active);
                    SetPrivateProperty(proj, nameof(Project.Budget), 100000m);
                    db.Projects.Add((Project)proj);
                    await db.SaveChangesAsync();
                    mainProject = (Project)proj;
                    Console.WriteLine("Seeded Project");
                }
                else
                {
                    mainProject = await db.Projects.FirstOrDefaultAsync();
                }

                // 3) ProjectTasks (for first up-to-10 employees)
                if (!await db.ProjectTasks.AnyAsync())
                {
                    var employees = await db.Employees.Take(10).ToListAsync();
                    if (employees.Any() && mainProject != null)
                    {
                        var tasks = new List<ProjectTask>();
                        int taskId = 1;
                        foreach (var emp in employees)
                        {
                            var pt = Activator.CreateInstance(typeof(ProjectTask));
                            SetPrivateProperty(pt, nameof(ProjectTask.ProjectId), mainProject.Id);
                            SetPrivateProperty(pt, nameof(ProjectTask.Title), $"Task {taskId++} for {emp.FirstName}");
                            SetPrivateProperty(pt, nameof(ProjectTask.Status), VertexHRMS.DAL.Enum.ProjectStatus.Active);
                            SetPrivateProperty(pt, nameof(ProjectTask.EstimatedHours), rnd.Next(1, 40));
                            SetPrivateProperty(pt, nameof(ProjectTask.StartDate), DateTime.Now.AddDays(-rnd.Next(1, 30)));
                            SetPrivateProperty(pt, nameof(ProjectTask.SpentHours), rnd.Next(0, 20));
                            SetPrivateProperty(pt, nameof(ProjectTask.DueDate), DateTime.Now.AddDays(rnd.Next(1, 60)));
                            SetPrivateProperty(pt, nameof(ProjectTask.AssignedToEmployeeId), emp.EmployeeId);
                            tasks.Add((ProjectTask)pt);
                        }
                        db.ProjectTasks.AddRange(tasks);
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Seeded {tasks.Count} ProjectTasks");
                    }
                    else
                    {
                        Console.WriteLine("No employees or project found to create ProjectTasks.");
                    }
                }

                // 4) EmployeeTrainings
                if (!await db.EmployeeTrainings.AnyAsync())
                {
                    var employees = await db.Employees.Take(20).ToListAsync();
                    var trainings = new List<EmployeeTraining>();
                    foreach (var emp in employees)
                    {
                        var tr = Activator.CreateInstance(typeof(EmployeeTraining));
                        SetPrivateProperty(tr, nameof(EmployeeTraining.EmployeeId), emp.EmployeeId);
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Employee), emp);
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Title), $"Orientation - {emp.FirstName}");
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Status), rnd.NextDouble() < 0.5 ? "Completed" : "Pending");
                        SetPrivateProperty(tr, nameof(EmployeeTraining.DueDate), DateTime.Now.AddDays(rnd.Next(1, 60)));
                        trainings.Add((EmployeeTraining)tr);
                    }
                    db.EmployeeTrainings.AddRange(trainings);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Seeded {trainings.Count} EmployeeTrainings");
                }

                // 5) Revenues (last 6 months)
                if (!await db.Revenues.AnyAsync())
                {
                    var revs = new List<Revenue>();
                    for (int m = 0; m < 6; m++)
                    {
                        var month = new DateTime(DateTime.Now.Year, Math.Max(1, DateTime.Now.Month - m), 1);
                        var r = Activator.CreateInstance(typeof(Revenue));
                        SetPrivateProperty(r, nameof(Revenue.MonthYear), month);
                        SetPrivateProperty(r, nameof(Revenue.Amount), 50000m + rnd.Next(0, 30000));
                        SetPrivateProperty(r, nameof(Revenue.Expenses), 20000m + rnd.Next(0, 15000));
                        revs.Add((Revenue)r);
                    }
                    db.Revenues.AddRange(revs);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Seeded Revenues");
                }

                // 6) LeaveEntitlements, LeaveLedger, LeaveRequest, LeaveRequestEmail
                // Ensure there is at least one LeaveType and some employees
                var firstLeaveType = await db.LeaveTypes.FirstOrDefaultAsync();
                var someEmployees = await db.Employees.Take(10).ToListAsync();
                if (firstLeaveType != null && someEmployees.Any())
                {
                    // Entitlements
                    if (!await db.LeaveEntitlements.AnyAsync())
                    {
                        var entList = new List<LeaveEntitlement>();
                        foreach (var emp in someEmployees)
                        {
                            var ent = Activator.CreateInstance(typeof(LeaveEntitlement), emp.EmployeeId, firstLeaveType.LeaveTypeId, DateTime.Now.Year, 21m, 0m, 0m);
                            entList.Add((LeaveEntitlement)ent);
                        }
                        db.LeaveEntitlements.AddRange(entList);
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Seeded {entList.Count} LeaveEntitlements");
                    }

                    // Ledger
                    if (!await db.LeaveLedgers.AnyAsync())
                    {
                        var ledgers = new List<LeaveLedger>();
                        foreach (var emp in someEmployees)
                        {
                            var lg = Activator.CreateInstance(typeof(LeaveLedger), emp.EmployeeId, firstLeaveType.LeaveTypeId, "Credit", 2m, DateTime.Now.AddMonths(-1));
                            ledgers.Add((LeaveLedger)lg);
                        }
                        db.LeaveLedgers.AddRange(ledgers);
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Seeded {ledgers.Count} LeaveLedgers");
                    }

                    // LeaveRequests + LeaveRequestDays + Approvals + LeaveRequestEmail
                    if (!await db.LeaveRequests.AnyAsync())
                    {
                        var leaveRequests = new List<LeaveRequest>();
                        foreach (var emp in someEmployees.Where((e, i) => i % 3 == 0))
                        {
                            var start = DateTime.Now.Date.AddDays(-rnd.Next(1, 30));
                            var end = start.AddDays(rnd.Next(1, 4));
                            var lr = new LeaveRequest(emp.EmployeeId, firstLeaveType.LeaveTypeId, start, end, "Approved", null);
                            leaveRequests.Add(lr);
                        }
                        db.LeaveRequests.AddRange(leaveRequests);
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Seeded {leaveRequests.Count} LeaveRequests");

                        // Days and approvals
                        var leaveDays = new List<LeaveRequestDay>();
                        var approvals = new List<LeaveApproval>();
                        foreach (var lr in leaveRequests)
                        {
                            var daysCount = Math.Max(1, (int)Math.Ceiling(lr.GetDurationInDays()));
                            var startDate = lr.StartDateTime;
                            for (int d = 0; d < daysCount; d++)
                            {
                                var theDate = startDate.AddDays(d);
                                var day = new LeaveRequestDay(lr.LeaveRequestId, theDate, 8m, false);
                                leaveDays.Add(day);
                            }
                            var approval = Activator.CreateInstance(typeof(LeaveApproval), lr.LeaveRequestId, 1, lr.EmployeeId, lr.Employee != null ? lr.Employee.IdentityUserId : null, "Approved", DateTime.Now);
                            approvals.Add((LeaveApproval)approval);
                        }
                        db.LeaveRequestDays.AddRange(leaveDays);
                        db.LeaveApprovals.AddRange(approvals);
                        await db.SaveChangesAsync();
                        Console.WriteLine("Seeded LeaveRequestDays and LeaveApprovals");

                        // LeaveRequestEmail
                        if (!await db.LeaveRequestEmails.AnyAsync())
                        {
                            var lreList = new List<LeaveRequestEmail>();
                            foreach (var lr in leaveRequests)
                            {
                                var email = new LeaveRequestEmail(lr.EmployeeId, "hr@company.com", "Leave request", "Please approve", DateTime.Now, false, lr.LeaveRequestId, "Re: Leave request", "Approved");
                                lreList.Add(email);
                            }
                            db.LeaveRequestEmails.AddRange(lreList);
                            await db.SaveChangesAsync();
                            Console.WriteLine($"Seeded {lreList.Count} LeaveRequestEmails");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Skipping leave-related seed: no LeaveType or no employees found.");
                }

                Console.WriteLine("Missing parts seeding finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("MissingPartsSeeder error: " + ex);
                throw;
            }
        }
    }

}
