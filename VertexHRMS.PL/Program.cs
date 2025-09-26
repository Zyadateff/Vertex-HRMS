using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// ------------------- Services -------------------
using Microsoft.Extensions.Options;
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
//Add Auto Mapper
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// Add services to the container.
builder.Services.AddControllersWithViews();
// Database
var connectionString = builder.Configuration.GetConnectionString("HRMS");
builder.Services.AddDbContext<VertexHRMSDbContext>(options =>
options.UseSqlServer(connectionString));
// Add session services
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = connectionString; 
    options.SchemaName = "dbo";                   
    options.TableName = "Sessions";              
});

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".VertexHRMS.Session";
    options.IdleTimeout = TimeSpan.FromHours(7);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Identity
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


// Cookie
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


// AutoMapper
builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

// Services & Repositories
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// HTTPS & Anti-forgery
builder.Services.AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect);
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//Dependancy injection
builder.Services.AddScoped<IAttendanceRecordsRepo, AttendanceRecordsRepo>();
builder.Services.AddScoped<IAttendanceRecordsService, AttendanceRecordsService>();
builder.Services.AddSingleton<IFaceRecognitionService, FaceRecognitionService>();
builder.Services.AddHttpClient<IAIService, AIService>();
builder.Services.AddScoped<IRevenueRepo, RevenueRepo>();
builder.Services.AddScoped<IProjectRepo, ProjectRepo>();
builder.Services.AddScoped<IProjectTaskRepo, ProjectTaskRepo>();
builder.Services.AddScoped<IEmployeeTrainingRepo, EmployeeTrainingRepo>();
builder.Services.AddScoped<IRevenueService, RevenueService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectTaskService, ProjectTaskService>();
builder.Services.AddScoped<IEmployeeTrainingService, EmployeeTrainingService>();
builder.Services.AddScoped<IDashboardRepo, DashboardRepo>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IProfileRepo, ProfileRepo>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug(); 
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });


var app = builder.Build();

// ------------------- Middleware -------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// Localization
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
