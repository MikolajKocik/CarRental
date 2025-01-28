using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace CarRental.Infrastructure.Data;

// ApplicationDbContext dziedziczy po IdentityDbContext, co oznacza, że obsługujemy także zarządzanie użytkownikami
public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        // Logowanie połączenia z bazą danych (do konsoli), przydatne w celach diagnostycznych
        Console.WriteLine($"Aktualne połączenie z bazą danych: {Database.GetDbConnection().ConnectionString}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly); 

        // assembly reference to all configurations classes in solution
    }
}
