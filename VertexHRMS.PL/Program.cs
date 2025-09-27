using Hangfire;
using Microsoft.EntityFrameworkCore;
using VertexHRMS.BLL.Mapper;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Service.Implementation;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Enhancement ConnectionString
var connectionString = builder.Configuration.GetConnectionString("HRMS");
builder.Services.AddDbContext<VertexHRMSDbContext>(options =>
options.UseSqlServer(connectionString));
// Repos
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
builder.Services.AddScoped<ILeaveRequestRepo, LeaveRequestRepository>();
builder.Services.AddScoped<ILeaveApprovalRepo, LeaveApprovalRepo>();
builder.Services.AddScoped<ILeaveTypeRepo, LeaveTypeRepo>();
builder.Services.AddScoped<ILeaveEntitlementRepo, LeaveEntitlementRepo>();
builder.Services.AddScoped<ILeaveLedgerRepo, LeaveLedgerRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<ILeaveRequestService,LeaveRequestService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IApplicationUserRepo, ApplicationUserRepo>();
builder.Services.AddScoped<ILeaveRequestEmailRepo, LeaveRequestEmailRepo>();
builder.Services.AddScoped<ILeaveEntitlementService, LeaveEntitlementService>();
builder.Services.AddScoped<ILeaveLedgerService, LeaveLedgerService>();
builder.Services.AddScoped<ILeaveRequestEmailService, LeaveRequestEmailService>();
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    app.UseHangfireDashboard("/EmailReading");
RecurringJob.AddOrUpdate<EmailService>(
    "check-inbox-job",
    x => x.CheckInboxAsync(),
    "*/2 * * * *"
);

app.Run();
