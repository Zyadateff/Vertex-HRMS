using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VertexHRMS.BLL.Mappers;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.BLL.Services.Implementation;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repositories.Abstraction;
using VertexHRMS.DAL.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Services -------------------
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<VertexHRMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services & Repositories
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// HTTPS & Anti-forgery
builder.Services.AddHttpsRedirection(options => options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect);
builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// ------------------- Middleware -------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ------------------- Routing -------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
