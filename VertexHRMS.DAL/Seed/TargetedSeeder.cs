using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Enum;

namespace VertexHRMS.DAL.Seed
{
    public class TargetedSeeder
    {
        private static void SetPrivateProperty(object target, string propertyName, object value)
        {
            if (target == null) return;
            var t = target.GetType();
            var prop = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null) throw new InvalidOperationException($"Property '{propertyName}' not found on type {t.FullName}");
            var setMethod = prop.GetSetMethod(true);
            if (setMethod == null)
                throw new InvalidOperationException($"Property '{propertyName}' on {t.FullName} has no setter (even non-public).");

            object assignValue = value;
            if (value != null)
            {
                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (!targetType.IsAssignableFrom(value.GetType()))
                {
                    assignValue = Convert.ChangeType(value, targetType);
                }
            }

            setMethod.Invoke(target, new[] { assignValue });
        }

        public static async Task SeedTargetedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var db = provider.GetRequiredService<VertexHRMSDbContext>();
            var rnd = new Random();

            Console.WriteLine("Starting TargetedSeeder (Revenues / Projects / Trainings / Calendar / Leaves) ...");

            // load employees (we will attach items to existing employees)
            var employees = await db.Employees.ToListAsync();
            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No employees found in DB. TargetedSeeder requires employees to attach trainings/tasks/leave. Aborting.");
                return;
            }

            // Ensure there's at least one leave type to reference
            var leaveTypes = await db.LeaveTypes.ToListAsync();
            if (leaveTypes == null || leaveTypes.Count == 0)
            {
                Console.WriteLine("No LeaveTypes found — inserting defaults used by FullSeeder style.");
                leaveTypes = new List<LeaveType>
                {
                    new LeaveType("Annual Leave", true, "Days"),
                    new LeaveType("Sick Leave", true, "Days"),
                    new LeaveType("Casual Leave", true, "Days")
                };
                db.LeaveTypes.AddRange(leaveTypes);
                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted {leaveTypes.Count} default leave types.");
            }

            // -------------------- Revenues --------------------
            try
            {
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
                    Console.WriteLine($"Inserted {revs.Count} revenues.");
                }
                else
                {
                    Console.WriteLine("Revenues already exist — skipping revenues seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Revenues seeding failed: " + ex.ToString());
            }

            // -------------------- Holiday Calendar & Holidays --------------------
            try
            {
                HolidayCalendar cal = await db.HolidayCalendars.FirstOrDefaultAsync(h => h.Name == "Default");
                if (cal == null)
                {
                    cal = new HolidayCalendar("Default");
                    db.HolidayCalendars.Add(cal);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Inserted HolidayCalendar 'Default'.");
                }

                var y = DateTime.Now.Year;
                var neededHolidays = new List<(DateTime dt, string name)>
                {
                    (new DateTime(y, 1, 1), "New Year"),
                    (new DateTime(y, 12, 25), "Christmas"),
                    (new DateTime(y, 1, 7), "Coptic Christmas"),
                    (new DateTime(y, 1, 25), "Revolution / Police Day"),
                    (new DateTime(y, 3, 21), "Eid al-Fitr"),
                    (new DateTime(y, 4, 13), "Sham El-Nessim"),
                    (new DateTime(y, 4, 25), "Sinai Liberation Day"),
                    (new DateTime(y, 5, 1), "Labour Day"),
                    (new DateTime(y, 5, 26), "Arrafat Day"),
                    (new DateTime(y, 5, 27), "Eid al-Adha"),
                    (new DateTime(y, 6, 19), "Islamic New Year"),
                    (new DateTime(y, 6, 30), "30 Jun Revolution"),
                    (new DateTime(y, 7, 23), "Revolution Day"),
                    (new DateTime(y, 8, 26), "Prophet Mohammad's Birthday"),
                    (new DateTime(y, 10, 6), "Armed Forces Day")
                };

                // fetch existing holidays for this calendar and extract their date property (support multiple property names)
                var existingEntities = await db.Holidays.Where(h => h.HolidayCalendarId == cal.HolidayCalendarId).ToListAsync();
                var existingDates = new HashSet<DateTime>();
                foreach (var he in existingEntities)
                {
                    DateTime? dt = null;
                    var ht = he.GetType();
                    var pHolidayDate = ht.GetProperty("HolidayDate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var pDate = ht.GetProperty("Date", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    var pTheDate = ht.GetProperty("Holiday", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); // fallback (unlikely)
                    if (pHolidayDate != null)
                    {
                        dt = pHolidayDate.GetValue(he) as DateTime? ?? (pHolidayDate.GetValue(he) is DateTime d ? d : (DateTime?)null);
                    }
                    else if (pDate != null)
                    {
                        dt = pDate.GetValue(he) as DateTime? ?? (pDate.GetValue(he) is DateTime d2 ? d2 : (DateTime?)null);
                    }
                    else if (pTheDate != null)
                    {
                        dt = pTheDate.GetValue(he) as DateTime? ?? (pTheDate.GetValue(he) is DateTime d3 ? d3 : (DateTime?)null);
                    }

                    if (dt.HasValue) existingDates.Add(dt.Value.Date);
                }

                var toAdd = new List<Holiday>();
                foreach (var (dt, name) in neededHolidays)
                {
                    if (!existingDates.Contains(dt.Date))
                    {
                        toAdd.Add(new Holiday(cal.HolidayCalendarId, dt, name));
                    }
                }

                if (toAdd.Count > 0)
                {
                    db.Holidays.AddRange(toAdd);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Inserted {toAdd.Count} holidays into calendar '{cal.Name}'.");
                }
                else
                {
                    Console.WriteLine("Holidays already present for Default calendar — skipping.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HolidayCalendar seeding failed: " + ex.ToString());
            }

            // -------------------- Projects & ProjectTasks --------------------
            try
            {
                if (!await db.Projects.AnyAsync(p => p.Name == "Internal HR System"))
                {
                    var proj = Activator.CreateInstance(typeof(Project));
                    SetPrivateProperty(proj, nameof(Project.Name), "Internal HR System");
                    SetPrivateProperty(proj, nameof(Project.StartDate), DateTime.Now.AddMonths(-3));
                    // set status using string — SetPrivateProperty will parse enum if necessary
                    SetPrivateProperty(proj, nameof(Project.Status), "Completed");
                    SetPrivateProperty(proj, nameof(Project.Budget), 100000m);
                    db.Projects.Add((Project)proj);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Inserted Project 'Internal HR System'.");

                    var createdProject = (Project)proj;
                    var tasks = new List<ProjectTask>();
                    int taskId = 1;
                    foreach (var emp in employees.Take(10))
                    {
                        var pt = Activator.CreateInstance(typeof(ProjectTask));
                        SetPrivateProperty(pt, nameof(ProjectTask.ProjectId), createdProject.Id);
                        SetPrivateProperty(pt, nameof(ProjectTask.Title), $"Task {taskId++} for {emp.FirstName}");
                        SetPrivateProperty(pt, nameof(ProjectTask.Status), "Active"); // will convert to enum if needed
                        SetPrivateProperty(pt, nameof(ProjectTask.EstimatedHours), rnd.Next(2, 40));
                        SetPrivateProperty(pt, nameof(ProjectTask.StartDate), DateTime.Now.AddDays(-rnd.Next(1, 30)));
                        SetPrivateProperty(pt, nameof(ProjectTask.SpentHours), rnd.Next(0, 20));
                        SetPrivateProperty(pt, nameof(ProjectTask.DueDate), DateTime.Now.AddDays(rnd.Next(1, 60)));
                        SetPrivateProperty(pt, nameof(ProjectTask.AssignedToEmployeeId), emp.EmployeeId);
                        tasks.Add((ProjectTask)pt);
                    }
                    db.ProjectTasks.AddRange(tasks);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Inserted {tasks.Count} project tasks for project '{createdProject.Name}'.");
                }
                else
                {
                    Console.WriteLine("Project 'Internal HR System' already exists — skipping projects seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Projects seeding failed: " + ex.ToString());
            }

            // -------------------- Employee Trainings --------------------
            try
            {
                var trainings = new List<EmployeeTraining>();
                foreach (var emp in employees)
                {
                    var has = await db.EmployeeTrainings.AnyAsync(t => t.EmployeeId == emp.EmployeeId);
                    if (!has)
                    {
                        var tr = Activator.CreateInstance(typeof(EmployeeTraining));
                        SetPrivateProperty(tr, nameof(EmployeeTraining.EmployeeId), emp.EmployeeId);
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Employee), emp);
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Title), $"Orientation - {emp.FirstName}");
                        SetPrivateProperty(tr, nameof(EmployeeTraining.Status), rnd.NextDouble() < 0.6 ? "Completed" : "Pending");
                        SetPrivateProperty(tr, nameof(EmployeeTraining.DueDate), DateTime.Now.AddDays(rnd.Next(1, 90)));
                        trainings.Add((EmployeeTraining)tr);

                        if (rnd.NextDouble() < 0.25)
                        {
                            var tr2 = Activator.CreateInstance(typeof(EmployeeTraining));
                            SetPrivateProperty(tr2, nameof(EmployeeTraining.EmployeeId), emp.EmployeeId);
                            SetPrivateProperty(tr2, nameof(EmployeeTraining.Employee), emp);
                            SetPrivateProperty(tr2, nameof(EmployeeTraining.Title), $"Compliance - {emp.FirstName}");
                            SetPrivateProperty(tr2, nameof(EmployeeTraining.Status), "Pending");
                            SetPrivateProperty(tr2, nameof(EmployeeTraining.DueDate), DateTime.Now.AddDays(rnd.Next(5, 120)));
                            trainings.Add((EmployeeTraining)tr2);
                        }
                    }
                }

                if (trainings.Count > 0)
                {
                    db.EmployeeTrainings.AddRange(trainings);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Inserted {trainings.Count} trainings.");
                }
                else
                {
                    Console.WriteLine("Employee trainings already exist for all employees — skipping trainings seeding.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Trainings seeding failed: " + ex.ToString());
            }

            // -------------------- Leave Entitlements, Ledgers, Requests, Days, Approvals --------------------
            try
            {
                var leaveEnts = new List<LeaveEntitlement>();
                var ledgers = new List<LeaveLedger>();
                var leaveRequests = new List<LeaveRequest>();

                // choose a base leave type (first available)
                var baseLeaveType = leaveTypes.First();

                foreach (var emp in employees)
                {
                    // add entitlement if not exists
                    var hasEnt = await db.LeaveEntitlements.AnyAsync(e => e.EmployeeId == emp.EmployeeId && e.Year == DateTime.Now.Year && e.LeaveTypeId == baseLeaveType.LeaveTypeId);
                    if (!hasEnt)
                    {
                        var ent = new LeaveEntitlement(emp.EmployeeId, baseLeaveType.LeaveTypeId, DateTime.Now.Year, 21m, 0m, rnd.Next(0, 5));
                        leaveEnts.Add(ent);
                    }

                    // ledger entry
                    var hasLedger = await db.LeaveLedgers.AnyAsync(l => l.EmployeeId == emp.EmployeeId && l.LeaveTypeId == baseLeaveType.LeaveTypeId);
                    if (!hasLedger)
                    {
                        var lg = new LeaveLedger(emp.EmployeeId, baseLeaveType.LeaveTypeId, "Credit", 2m, DateTime.Now.AddMonths(-1));
                        ledgers.Add(lg);
                    }

                    // sometimes create a leave request
                    if (rnd.NextDouble() < 0.25)
                    {
                        var start = DateTime.Now.Date.AddDays(-rnd.Next(1, 30));
                        var end = start.AddDays(rnd.Next(0, 3));
                        var status = rnd.NextDouble() < 0.6 ? "Approved" : "Pending";
                        var lr = new LeaveRequest(emp.EmployeeId, baseLeaveType.LeaveTypeId, start, end, status, emp.IdentityUserId);
                        leaveRequests.Add(lr);
                    }
                }

                if (leaveEnts.Count > 0)
                {
                    db.LeaveEntitlements.AddRange(leaveEnts);
                }
                if (ledgers.Count > 0)
                {
                    db.LeaveLedgers.AddRange(ledgers);
                }
                if (leaveRequests.Count > 0)
                {
                    db.LeaveRequests.AddRange(leaveRequests);
                }

                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted LeaveEntitlements: {leaveEnts.Count}, Ledgers: {ledgers.Count}, LeaveRequests: {leaveRequests.Count}");

                // now create LeaveRequestDays and LeaveApprovals for created leaveRequests
                var leaveDays = new List<LeaveRequestDay>();
                var leaveApprovals = new List<LeaveApproval>();
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

                    if (string.Equals(lr.Status, "Approved", StringComparison.OrdinalIgnoreCase))
                    {
                        var approval = new LeaveApproval(lr.LeaveRequestId, 1, lr.EmployeeId, lr.Employee != null ? lr.Employee.IdentityUserId : null, "Approved", DateTime.Now);
                        leaveApprovals.Add(approval);
                    }
                }

                if (leaveDays.Count > 0) db.LeaveRequestDays.AddRange(leaveDays);
                if (leaveApprovals.Count > 0) db.LeaveApprovals.AddRange(leaveApprovals);

                if (leaveDays.Count > 0 || leaveApprovals.Count > 0)
                {
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Inserted LeaveRequestDays: {leaveDays.Count}, LeaveApprovals: {leaveApprovals.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Leave seeding failed: " + ex.ToString());
            }

            Console.WriteLine("TargetedSeeder finished.");
        }
    }
}
