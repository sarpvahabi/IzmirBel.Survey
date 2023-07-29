using IzmirBel.Survey.Models.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IzmirBel.Survey.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using IzmirBel.Survey.Services;
using IzmirBel.Survey.Settings;
using SendGrid.Extensions.DependencyInjection;
using SendGrid;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SurveyDbContext>(
    options => options
        .UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<SurveyIdentityDbContext>(
    options => options
        .UseSqlServer(connectionString: builder.Configuration.GetConnectionString("IdentityConnection")));

//Added with scaffolding Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<SurveyIdentityDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 12; //ASVS 2.1.1
    options.Password.RequireDigit = false; //ASVS 2.1.9
    options.Password.RequireLowercase = false; //ASVS 2.1.9
    options.Password.RequireNonAlphanumeric = false; //ASVS 2.1.9
});

builder.Services.Configure<SendGridSettings>(config: builder.Configuration.GetSection("SendGridSettings"));

builder.Services.AddSendGrid(options => {
    options.ApiKey = builder.Configuration.GetSection("SendGridSettings")
    .GetValue<string>("ApiKey");
});

builder.Services.AddScoped<IEmailSender, EmailSender>(); //AddTransient

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

app.MapRazorPages();

app.Run();
