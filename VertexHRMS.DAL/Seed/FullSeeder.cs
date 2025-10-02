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

namespace VertexHRMS.DAL.Seed
{
    public class FullSeeder
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
            setMethod.Invoke(target, new[] { value });
        }

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var provider = scope.ServiceProvider;

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var db = provider.GetRequiredService<VertexHRMSDbContext>();
            var env = provider.GetService<IWebHostEnvironment>(); // might be null in some contexts

            // Guard: don't reseed if employees exist
            if (await db.Employees.AnyAsync())
            {
                Console.WriteLine("Seed aborted: Employees already exist.");
                return;
            }

            Console.WriteLine("Starting FullSeeder...");

            var rnd = new Random();

            try
            {
                // Roles
                var roles = new[] { "HR", "Employee" };
                foreach (var r in roles)
                {
                    if (!await roleManager.RoleExistsAsync(r))
                    {
                        var rr = await roleManager.CreateAsync(new IdentityRole(r));
                        Console.WriteLine($"Created role {r}: {rr.Succeeded}");
                    }
                }

                // Departments
                var deptNames = new[]
                {
                    "Management board", "Q&A", "Marketing", "Operations", "Sales",
                    "Finance", "HR", "R&D", "IT", "Engineering"
                };
                var departments = deptNames.Select(n => new Department(n)).ToList();
                db.Departments.AddRange(departments);
                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted {departments.Count} departments.");

                // Positions
                var positionList = new[]
                {
                    new Position("CEO", 20000m),
                    new Position("Manager", 8000m),
                    new Position("Senior Engineer", 9000m),
                    new Position("Engineer", 6000m),
                    new Position("QA Engineer", 5000m),
                    new Position("HR Specialist", 4500m),
                    new Position("Accountant", 4800m),
                    new Position("Marketing Specialist", 4700m),
                    new Position("Sales Rep", 4000m),
                    new Position("Support", 3500m)
                }.ToList();
                db.Positions.AddRange(positionList);
                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted {positionList.Count} positions.");

                // WorkSchedules
                var schedules = new[]
                {
                    new WorkSchedule("Morning", new TimeSpan(8,0,0), new TimeSpan(15,0,0)),
                    new WorkSchedule("Evening", new TimeSpan(15,0,0), new TimeSpan(22,0,0))
                };
                db.WorkSchedules.AddRange(schedules);
                await db.SaveChangesAsync();
                Console.WriteLine("Inserted work schedules.");

                // LeaveTypes, Policies, Deductions, Holidays
                var leaveTypes = new[]
                {
                    new LeaveType("Annual Leave", true, "Days"),
                    new LeaveType("Sick Leave", true, "Days"),
                    new LeaveType("Unpaid Leave", false, "Days"),
                    new LeaveType("ANNUAL", true, "days"),
                    new LeaveType("SICK", true, "days"),
                    new LeaveType("UNPAID", false, "days"),
                    new LeaveType("CASUAL", true, "days"),
                    new LeaveType("MATERNITY", true, "days"),
                    new LeaveType("SUMMER", true, "days")
                }.ToList();
                db.LeaveTypes.AddRange(leaveTypes);
                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted {leaveTypes.Count} leave types.");

                var policies = new[]
                {
                    new LeavePolicy(leaveTypes[0].LeaveTypeId, "Yearly", 21),
                    new LeavePolicy(leaveTypes[1].LeaveTypeId, "AsNeeded", 10)
                }.ToList();
                db.LeavePolicies.AddRange(policies);
                await db.SaveChangesAsync();

                var deductions = new[]
                {
                    new Deduction("Tax", true, 10m),
                    new Deduction("Insurance", true, 10m),
                }.ToList();
                db.Deductions.AddRange(deductions);
                await db.SaveChangesAsync();

                var cal = new HolidayCalendar("Default");
                db.HolidayCalendars.Add(cal);
                await db.SaveChangesAsync();

                var y = DateTime.Now.Year;
                var holidays = new List<Holiday>
                {
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 1, 1), "New Year"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 12, 25), "Christmas"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 1, 7), "Coptic Christmas"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 1, 25), "Revolution / Police Day"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 3, 21), "Eid al-Fitr"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 4, 13), "Sham El-Nessim"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 4, 25), "Sinai Liberation Day"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 5, 1), "Labour Day"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 5, 26), "Arrafat Day"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 5, 27), "Eid al-Adha"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 6, 19), "Islamic New Year"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 6, 30), "30 Jun Revolution"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 7, 23), "Revolution Day"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 8, 26), "Prophet Mohammad's Birthday"),
                    new Holiday(cal.HolidayCalendarId, new DateTime(y, 10, 6), "Armed Forces Day")
                };
                db.Holidays.AddRange(holidays);
                await db.SaveChangesAsync();
                Console.WriteLine($"Inserted {holidays.Count} holidays.");

                // -------------------- Create Users + Employees --------------------
                var names = new (string First, string Last)[]
                {
                    ("Abdallah","Qusit"),("Ibrahim","Abdelmohsen"),("Zyad","Mohamed"),("Mina","Sabir"),("Farouk","Ibrahim"),
                    ("Youssef","Kamal"),("Karim","Adel"),("Hany","Fathy"),("Omar","Naguib"),("Ali","Farouk"),
                    ("Nour","Khaled"),("Ahmed","Samir"),("Mahmoud","Tarek"),("Sara","Omar"),("Hassan","Ali"),
                    ("Amr","Saeed"),("Khalid","Nabil"),("Sabry","Yasser"),("Adel","Hossam"),("Essam","Khalil"),
                    ("Fatma","Said"),("Tamer","Mostafa"),("Walid","Omar"),("Khaled","Anwar"),("Maria","Rizk"),
                    ("John","Smith"),("Pip","Lopez"),("Li","Wei"),("Amr","Hazzem"),("David","Brown"),
                    ("James","Brown"),("Laura","Garcia"),("Isabella","Silva"),("Mateo","Lopez"),("Sven","Andersson"),
                    ("Lara","Said"),("Rana","ElMasry"),("Ivy","Ng"),("Ola","Boulos"),("Salma","Azoz"),
                    ("Hassan","Morsi"),("Walid","Nader"),("Mariam","Fouad"),("Basma","Magdy"),("Zein","Abdel"),
                    ("Peter","Miller"),("Lucas","Davis"),("Aisha","Kamal"),("Samer","Hani"),("Rita","Aziz")
                };

                var domains = new[] { "gmail.com", "outlook.com", "hotmail.com", "vertex.hrms" };

                var employees = new List<Employee>();
                var createdUsers = new List<ApplicationUser>();
                int seq = 1;
                var failedUsers = new List<string>();

                foreach (var (First, Last) in names)
                {
                    string localPart = $"{First}.{Last}".ToLower().Replace(" ", "");
                    string domain = domains[rnd.Next(domains.Length)];
                    string email = $"{localPart}{(rnd.Next(100) > 60 ? rnd.Next(10, 999) : "")}@{domain}";

                    // avoid duplicates db + local list
                    while (await db.Users.AnyAsync(u => u.Email == email) || createdUsers.Any(u => u.Email == email))
                    {
                        email = $"{localPart}{rnd.Next(1000)}@{domain}";
                    }

                    var user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        MustChangePassword = false,
                        EmailConfirmed = true
                    };

                    var pw = "P@ssw0rd!";
                    var createUserResult = await userManager.CreateAsync(user, pw);
                    if (!createUserResult.Succeeded)
                    {
                        var errs = string.Join(" | ", createUserResult.Errors.Select(x => x.Description));
                        Console.WriteLine($"User create failed for {email}: {errs}");
                        failedUsers.Add(email);
                        continue; // skip this candidate
                    }

                    string role = (rnd.NextDouble() < 0.08) ? "HR" : "Employee";
                    await userManager.AddToRoleAsync(user, role);

                    var dept = departments[rnd.Next(departments.Count)];
                    var pos = positionList[rnd.Next(positionList.Count)];

                    var code = $"EMP{seq.ToString().PadLeft(4, '0')}";

                    // hashed prefix + sequential employee number (DB will store path)
                    var randomPrefix = Guid.NewGuid().ToString("N");
                    var imgName = $"{randomPrefix}_employee{seq}.jpg";
                    var imgPath = $"/Files/{imgName}";

                    // try to copy original physical file if exists (employee{seq}.jpg)
                    try
                    {
                        if (env != null)
                        {
                            var filesFolder = Path.Combine(env.ContentRootPath, "wwwroot", "Files");
                            var originalFile = Path.Combine(filesFolder, $"employee{seq}.jpg");
                            if (File.Exists(originalFile))
                            {
                                var destPath = Path.Combine(filesFolder, imgName);
                                File.Copy(originalFile, destPath, overwrite: true);
                            }
                            else
                            {
                                // original not found: that's fine, DB will point to hashed path anyway
                                // optionally log: Console.WriteLine($"Original image not found: {originalFile}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Image copy failed for seq {seq}: {ex.Message}");
                    }

                    string phone = (rnd.Next(10, 99)).ToString() + (rnd.Next(10000000, 99999999)).ToString();

                    var employee = Employee.Create(
                        employeeCode: code,
                        firstName: First,
                        lastName: Last,
                        email: email,
                        phone: phone,
                        departmentId: dept.DepartmentId,
                        positionId: pos.PositionId,
                        identityUserId: user.Id,
                        imagePath: imgPath,
                        managerId: null
                    );

                    employees.Add(employee);
                    createdUsers.Add(user);
                    seq++;
                }

                // Save employees (parents) first
                db.Employees.AddRange(employees);
                await db.SaveChangesAsync();
                Console.WriteLine($"Created employees: {employees.Count}, failed users: {failedUsers.Count}");

                // -------------------- Payroll run + Payrolls + Deductions --------------------
                try
                {
                    var payrollRun = new PayrollRun(DateTime.Now.AddMonths(-1).Date, DateTime.Now.Date, DateTime.Now);
                    db.PayrollRuns.Add(payrollRun);
                    await db.SaveChangesAsync();

                    var payrolls = new List<Payroll>();
                    foreach (var emp in employees)
                    {
                        decimal baseSalary = (decimal)(emp.Salary ?? 3000);
                        decimal gross = baseSalary + 200;
                        decimal tax = deductions[0].IsPercentage ? gross * deductions[0].AmountOrPercent / 100m : deductions[0].AmountOrPercent;
                        decimal ins = deductions[1].IsPercentage ? gross * deductions[1].AmountOrPercent / 100m : deductions[1].AmountOrPercent;
                        decimal net = gross - tax - ins;

                        var payroll = new Payroll(payrollRun.PayrollRunId, emp.EmployeeId, baseSalary, gross, net, DateTime.Now);
                        payrolls.Add(payroll);
                    }
                    db.Payrolls.AddRange(payrolls);
                    await db.SaveChangesAsync();

                    var pds = new List<PayrollDeduction>();
                    foreach (var p in payrolls)
                    {
                        var d1 = new PayrollDeduction(p.PayrollId, deductions[0].DeductionId, Math.Round(p.GrossEarnings * deductions[0].AmountOrPercent / 100m, 2));
                        pds.Add(d1);
                    }
                    db.PayrollDeductions.AddRange(pds);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Payroll seeding failed: " + ex);
                }

                // -------------------- Attendance --------------------
                try
                {
                    var attendances = new List<AttendanceRecord>();
                    foreach (var emp in employees)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var day = DateTime.Now.Date.AddDays(-rnd.Next(1, 30));
                            var checkIn = day.AddHours(8 + rnd.NextDouble());
                            var checkOut = checkIn.AddHours(7 + rnd.NextDouble());
                            var workMinutes = (decimal)(checkOut - checkIn).TotalMinutes;
                            var ar = new AttendanceRecord(emp.EmployeeId, day, checkIn, checkOut, workMinutes, "Present");
                            attendances.Add(ar);
                        }
                    }
                    db.AttendanceRecords.AddRange(attendances);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Attendance seeding failed: " + ex);
                }

                // -------------------- Leave entitlements & requests (parents then children) --------------------
                try
                {
                    var leaveEnts = new List<LeaveEntitlement>();
                    var ledgers = new List<LeaveLedger>();
                    var leaveRequests = new List<LeaveRequest>();

                    foreach (var emp in employees)
                    {
                        var ent = new LeaveEntitlement(emp.EmployeeId, leaveTypes[0].LeaveTypeId, DateTime.Now.Year, 21m, 0m, rnd.Next(0, 5));
                        leaveEnts.Add(ent);

                        var lg = new LeaveLedger(emp.EmployeeId, leaveTypes[0].LeaveTypeId, "Credit", 2m, DateTime.Now.AddMonths(-1));
                        ledgers.Add(lg);

                        if (rnd.NextDouble() < 0.25)
                        {
                            var start = DateTime.Now.Date.AddDays(-rnd.Next(1, 30));
                            var end = start.AddDays(rnd.Next(1, 4));
                            var lr = new LeaveRequest(emp.EmployeeId, leaveTypes[0].LeaveTypeId, start, end, "Approved", null);
                            leaveRequests.Add(lr);
                        }
                    }

                    db.LeaveEntitlements.AddRange(leaveEnts);
                    db.LeaveLedgers.AddRange(ledgers);
                    db.LeaveRequests.AddRange(leaveRequests);
                    await db.SaveChangesAsync();

                    // now children: days and approvals
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

                        var approval = new LeaveApproval(lr.LeaveRequestId, 1, lr.EmployeeId, lr.Employee != null ? lr.Employee.IdentityUserId : null, "Approved", DateTime.Now);
                        leaveApprovals.Add(approval);
                    }

                    db.LeaveRequestDays.AddRange(leaveDays);
                    db.LeaveApprovals.AddRange(leaveApprovals);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Leave seeding failed: " + ex);
                }

                // -------------------- Trainings --------------------
                try
                {
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Trainings seeding failed: " + ex);
                }

                // -------------------- Projects & tasks --------------------
                try
                {
                    var proj = Activator.CreateInstance(typeof(Project));
                    SetPrivateProperty(proj, nameof(Project.Name), "Internal HR System");
                    SetPrivateProperty(proj, nameof(Project.StartDate), DateTime.Now.AddMonths(-3));
                    SetPrivateProperty(proj, nameof(Project.Status), VertexHRMS.DAL.Enum.ProjectStatus.Active);
                    SetPrivateProperty(proj, nameof(Project.Budget), 100000m);
                    db.Projects.Add((Project)proj);
                    await db.SaveChangesAsync();

                    var tasks = new List<ProjectTask>();
                    int taskId = 1;
                    foreach (var emp in employees.Take(10))
                    {
                        var pt = Activator.CreateInstance(typeof(ProjectTask));
                        SetPrivateProperty(pt, nameof(ProjectTask.ProjectId), ((Project)proj).Id);
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Projects seeding failed: " + ex);
                }

                // -------------------- Applicants -> Interviews/Offers/Onboardings --------------------
                try
                {
                    var jobOpening = new JobOpening(positionList[3].PositionId, departments[3].DepartmentId, "Junior Engineer", DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30), "Open");
                    db.JobOpenings.Add(jobOpening);
                    await db.SaveChangesAsync();

                    var applicants = new List<Applicant>();
                    for (int i = 0; i < Math.Min(15, employees.Count); i++)
                    {
                        var emp = employees[i];
                        var appliedDate = DateTime.Now.AddDays(-rnd.Next(1, 90));
                        var applicant = new Applicant(jobOpening.JobOpeningId, emp.FirstName, emp.LastName, emp.Email, $"Files/{Guid.NewGuid().ToString("N").Substring(0, 32)}.pdf", appliedDate, "Applied", emp.IdentityUserId);
                        applicants.Add(applicant);
                    }
                    db.Applicants.AddRange(applicants);
                    await db.SaveChangesAsync();

                    var interviews = new List<Interview>();
                    var offers = new List<Offer>();
                    var onboardings = new List<Onboarding>();
                    for (int i = 0; i < applicants.Count; i++)
                    {
                        var a = applicants[i];
                        if (i % 2 == 0)
                        {
                            var interviewer = employees[rnd.Next(employees.Count)];
                            var interviewDate = a.AppliedDate.AddDays(7);
                            var interview = new Interview(a.ApplicantId, jobOpening.JobOpeningId, interviewDate, interviewer.EmployeeId, "Good fit", interviewer.IdentityUserId);
                            interviews.Add(interview);
                        }
                        if (rnd.NextDouble() < 0.3)
                        {
                            var offerDate = a.AppliedDate.AddDays(10);
                            var offeredSalary = 5000m + rnd.Next(0, 2000);
                            var joiningDate = offerDate.AddDays(14);
                            var issuer = employees[rnd.Next(employees.Count)];
                            var offer = new Offer(a.ApplicantId, jobOpening.JobOpeningId, offerDate, offeredSalary, joiningDate, "Issued", issuer.IdentityUserId);
                            offers.Add(offer);
                            if (rnd.NextDouble() < 0.5)
                            {
                                var onboard = new Onboarding(a.ApplicantId, 0, joiningDate, false, issuer.IdentityUserId);
                                onboardings.Add(onboard);
                            }
                        }
                    }
                    db.Interviews.AddRange(interviews);
                    db.Offers.AddRange(offers);
                    db.Onboardings.AddRange(onboardings);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Applicants/Offers seeding failed: " + ex);
                }

                // -------------------- Resignations & ExitClearances --------------------
                try
                {
                    var resigns = new List<Resignation>();
                    var exits = new List<ExitClearance>();
                    foreach (var emp in employees.Where((e, i) => i % 20 == 0))
                    {
                        var notice = DateTime.Now.AddDays(-30);
                        var last = notice.AddDays(14);
                        var req = new Resignation(emp.EmployeeId, notice, last, "Pending", emp.IdentityUserId);
                        resigns.Add(req);
                    }
                    db.Resignations.AddRange(resigns);
                    await db.SaveChangesAsync();

                    foreach (var r in resigns)
                    {
                        var ec = new ExitClearance(r.ResignationId, true, true, false, 1200m, r.RequestedByUserId, r.RequestedByUserId, r.RequestedByUserId);
                        exits.Add(ec);
                    }
                    db.ExitClearances.AddRange(exits);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Resignations seeding failed: " + ex);
                }

                // -------------------- Revenues --------------------
                try
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Revenues seeding failed: " + ex);
                }

                Console.WriteLine("Seeding finished.");
            }
            catch (Exception outerEx)
            {
                Console.WriteLine("FullSeeder fatal error: " + outerEx);
                throw;
            }
        }
    }
}
