using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using VertexHRMS.BLL.Mapper;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Service.Implementation;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.BLL.Services.Implementation;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.PL.Language;

var builder = WebApplication.CreateBuilder(args);

// ------------------- AutoMapper -------------------
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// ------------------- ATS Integration -------------------
builder.Services.AddHttpClient<IResumeParser, SimpleResumeParser>();

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
builder.Services.AddDbContext<VertexHRMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMS"))); // make sure "HRMS" exists in appsettings.json

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

// ------------------- Team B Services -------------------
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// ------------------- ATS Services -------------------
builder.Services.AddScoped<IFormIngestService, FormIngestService>();
builder.Services.AddScoped<IATSPipeline, ATSPipeline>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
