using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace IzmirBel.Survey.Data;

public class SurveyIdentityDbContext : IdentityDbContext<IdentityUser>
{
    public SurveyIdentityDbContext(DbContextOptions<SurveyIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        var adminRole = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Administrator",
            ConcurrencyStamp = "1",
            NormalizedName = "ADMINISTRATOR"
        };

        builder.Entity<IdentityRole>().HasData(adminRole);

        //bu rolde olacak bir kullanıcı ekleyelim.
        var roleUser = CreateUser("admin@izmir.bel.tr");
        builder.Entity<IdentityUser>().HasData(roleUser);

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRole.Id,
            UserId = roleUser.Id
        });

        var claimUser = CreateUser("gm@izmir.bel.tr");

        builder.Entity<IdentityUserClaim<string>>().HasData(new IdentityUserClaim<string>
        {
            Id= 1,
            UserId = claimUser.Id.ToString(),
            ClaimType="IsManager",
            ClaimValue ="true"
        }); 
    }

    private IdentityUser CreateUser(string email) =>
        new IdentityUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = email.ToLower(),
            NormalizedEmail = email.ToUpper(),
            UserName = email.ToLower(),
            NormalizedUserName = email.ToUpper(),
            EmailConfirmed = true,
            ConcurrencyStamp = "1"
        };
}
