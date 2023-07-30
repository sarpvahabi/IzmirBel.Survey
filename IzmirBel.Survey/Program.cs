using AspNetCoreRateLimit;
using IzmirBel.Survey.ActionFilters;
using IzmirBel.Survey.Data;
using IzmirBel.Survey.Models.Data;
using IzmirBel.Survey.Services;
using IzmirBel.Survey.Settings;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SendGrid.Extensions.DependencyInjection;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddSimpleConsole(options =>
//{
//    options.UseUtcTimestamp = true;
//    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss.fff]";
//});

// Add services to the container.
builder.Services.AddControllersWithViews(config=>
{
    config.Filters.Add<ModelStateValidationFilter>();
});

builder.Services.AddDbContext<SurveyDbContext>(
    options => options
        .UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SurveyIdentityDbContext>(
    options => options
        .UseSqlServer(connectionString: builder.Configuration.GetConnectionString("IdentityConnection")));

//Added with scaffolding Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<SurveyIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //ASVS 3.4.1
    options.Cookie.HttpOnly = true; //ASVS 3.4.2
    options.Cookie.SameSite = SameSiteMode.Strict; //ASVS 3.4.3
    options.Cookie.Name = "__Host-Identity"; //ASVS 3.4.4
    options.Cookie.Path = "/"; //ASVS 3.4.5
    options.Cookie.MaxAge = TimeSpan.FromHours(12);
    options.ExpireTimeSpan = TimeSpan.FromHours(12);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 12; //ASVS 2.1.1
    options.Password.RequireDigit = false; //ASVS 2.1.9
    options.Password.RequireUppercase = false; //ASVS 2.1.9
    options.Password.RequireLowercase = false; //ASVS 2.1.9
    options.Password.RequireNonAlphanumeric = false; //ASVS 2.1.9
    options.Lockout.MaxFailedAccessAttempts = 5; //ASVS 2.2.1
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //ASVS 2.2.1
});

builder.Services.Configure<SendGridSettings>(config: builder.Configuration.GetSection("SendGridSettings"));

builder.Services.AddSendGrid(options => {
    options.ApiKey = builder.Configuration.GetSection("SendGridSettings")
    .GetValue<string>("ApiKey");
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = Convert.ToInt64(builder.Configuration["MaxFileSize"]);
});

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHsts(options =>
    {
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromMinutes(365);
    });

    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
        options.HttpsPort = 443;
    });
}
else
{
    builder.Services.AddHsts(options =>
    {
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromMinutes(365);
    }); 
    
    builder.Services.AddHttpsRedirection(options =>
    {
        options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
        options.HttpsPort = 7078;
    });

}

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddTransient<FileValidationService>();

builder.Services.AddDistributedMemoryCache(); //redis tarzý bir data store yok, bunu yaparak session'larý memory'de tutacaðýz.

builder.Services.Configure<ClientRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            QuotaExceededResponse = new QuotaExceededResponse{ Content = "Please only submit responses one time."},
            Endpoint = "POST:/Survey/TestSurvey/*", //wildcard
            Period= "10m",
            Limit = 1
        }
     };
});

builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //ASVS 3.4.1
    options.Cookie.HttpOnly = true; //ASVS 3.4.2
    options.Cookie.SameSite = SameSiteMode.Strict; //ASVS 3.4.3
    options.Cookie.Name = "__Host-Session"; //ASVS 3.4.4
    options.Cookie.Path = "/"; //ASVS 3.4.5
    options.Cookie.MaxAge = TimeSpan.FromHours(1);
    options.IdleTimeout = TimeSpan.FromMinutes(20);
});
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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapAreaControllerRoute(
    name:"admin",
    areaName:"Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
