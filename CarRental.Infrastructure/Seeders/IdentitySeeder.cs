using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CarRental.Infrastructure.Seeders;

public class IdentitySeeder : IIdentitySeeder
{
    private readonly ApplicationDbContext _context;
    public IdentitySeeder(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task SeederIdentityAsync(RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager, ILogger<IdentityUser> logger)
    {

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                if (!_context.Users.Any(u => u.Email == "admin@example.com"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin"); // Add Admin to role
                        logger.LogInformation("Admin account created successfully.");
                    }
                    else
                    {
                        logger.LogError("Failed to create admin account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }

                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "There was a problem during seeding a identity data");
                throw;
            }
        }
    }
}
