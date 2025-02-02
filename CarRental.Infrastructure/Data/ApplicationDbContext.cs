using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data;

// ApplicationDbContext inherit from IdentityDbContext, what defines that we also manage identity users in db
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Report> Reports { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Information about connection to database with console, diagnostics purposes
        Console.WriteLine($"Actually connection with database: {Database.GetDbConnection().ConnectionString}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey });

        // assembly reference to all configurations classes in solution

        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

      
    }
}
