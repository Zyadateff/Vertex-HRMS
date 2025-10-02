using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;
using VertexHRMS.BLL.Mapper;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Service.Implementation;
using VertexHRMS.BLL.Service.Implementation;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.BLL.Services.Implementation;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;
using VertexHRMS.DAL.Seed;
using VertexHRMS.PL.Language;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// ------------------- AutoMapper -------------------
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// ------------------- MVC & Localization -------------------
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });

// ------------------- Session -------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ------------------- Database -------------------


var connectionString = builder.Configuration.GetConnectionString("HRMS");
builder.Services.AddDbContext<VertexHRMSDbContext>(options =>
    options.UseSqlServer(connectionString));

// Hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer(); 


// ------------------- Identity -------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<VertexHRMSDbContext>()
.AddDefaultTokenProviders();

// ------------------- Cookie -------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
// Repos
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));
builder.Services.AddScoped<ILeaveRequestRepo, LeaveRequestRepository>();
builder.Services.AddScoped<ILeaveTypeRepo, LeaveTypeRepo>();
builder.Services.AddScoped<ILeaveEntitlementRepo, LeaveEntitlementRepo>();
builder.Services.AddScoped<ILeaveLedgerRepo, LeaveLedgerRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILeaveRequestEmailRepo, LeaveRequestEmailRepo>();
builder.Services.AddScoped<ILeaveEntitlementService, LeaveEntitlementService>();
builder.Services.AddScoped<ILeaveLedgerService, LeaveLedgerService>();
builder.Services.AddScoped<ILeaveRequestEmailService, LeaveRequestEmailService>();

// ------------------- Core Services & Repositories -------------------
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IAttendanceRecordsRepo, AttendanceRecordsRepo>();
builder.Services.AddScoped<IAttendanceRecordsService, AttendanceRecordsService>();

builder.Services.AddSingleton<IFaceRecognitionService, FaceRecognitionService>();
builder.Services.AddHttpClient<IAIService, AIService>();

// ------------------- Team A Services -------------------
builder.Services.AddScoped<IRevenueRepo, RevenueRepo>();
builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
builder.Services.AddScoped<IProjectTaskRepo, ProjectTaskRepo>();
builder.Services.AddScoped<IEmployeeTrainingRepo, EmployeeTrainingRepo>();
builder.Services.AddScoped<IRevenueService, RevenueService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<IEmployeeTrainingService, EmployeeTrainingService>();
builder.Services.AddScoped<IDepartmentCardsRepo, DepartmentCardsRepo>();
builder.Services.AddScoped<IDepartmentCardsService, DepartmentCardsService>();
builder.Services.AddScoped<IEmployeeCardsRepo, EmployeeCardsRepo>();
builder.Services.AddScoped<IEmployeeCardsService, EmployeeCardsService>();
builder.Services.AddScoped<IEmployeeDescriptionRepo, EmployeeDescriptionRepo>();
builder.Services.AddScoped<IEmployeeDescriptionService, EmployeeDescriptionService>();

// Payroll Repos
builder.Services.AddScoped<IDeductionRepo, DeductionRepo>();
builder.Services.AddScoped<IPayrollRepo, PayrollRepo>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IPayrollRunRepo, PayrollRunRepo>();
builder.Services.AddScoped<IPayrollRunService, PayrollRunService>();
builder.Services.AddScoped<IPayrollDeductionRepo, PayrollDeductionRepo>();
builder.Services.AddScoped<IPayrollEmailService, PayrollEmailService>();

// ------------------- Team B Services -------------------
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProfileRepo, ProfileRepo>();
builder.Services.AddScoped<IProfileService, ProfileService>();
// ------------------- ATS Services -------------------
builder.Services.AddScoped<IFormIngestService, FormIngestService>();
builder.Services.AddScoped<IExternalFormDb, DemoExternalFormDb>();
builder.Services.AddScoped<IFileStore>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new LocalFileStore(env.WebRootPath ?? Path.GetTempPath());
});

// ------------------- HTTPS & Antiforgery -------------------
builder.Services.AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect);
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

// ------------------- Logging -------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// ------------------- Middleware -------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var supportedCultures = new[] {
    new CultureInfo("ar-EG"),
    new CultureInfo("en-US"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// ------------------- Routing -------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DepartmentCards}/{action=Getdepart}/{id?}");
    app.UseHangfireDashboard("/EmailReading");
RecurringJob.AddOrUpdate<EmailService>(
    "check-inbox-job",
    x => x.CheckInboxAsync(),
    "*/1 * * * *"
);
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    await FullSeeder.SeedAsync(services);
//}
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    await TargetedSeeder.SeedTargetedAsync(services);
//}
//using (var scope = app.Services.CreateScope())
//{
//    await MissingPartsSeeder.SeedMissingAsync(scope.ServiceProvider);
//}
app.Run();
