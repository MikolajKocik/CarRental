using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Data
{
    // ApplicationDbContext dziedziczy po IdentityDbContext, co oznacza, że obsługujemy także zarządzanie użytkownikami
    public class ApplicationDbContext : IdentityDbContext
    {
        // DbSet reprezentujące tabelę Cars w bazie danych
        public DbSet<Car> Cars { get; set; }

        // DbSet reprezentujące tabelę Reservations w bazie danych
        public DbSet<Reservation> Reservations { get; set; }

        // Konstruktor odbierający opcje konfiguracji bazy danych, które będą przekazywane do bazowej klasy IdentityDbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Logowanie połączenia z bazą danych (do konsoli), przydatne w celach diagnostycznych
            Console.WriteLine($"Aktualne połączenie z bazą danych: {Database.GetDbConnection().ConnectionString}");
        }

        // Konfiguracja modelu bazy danych (np. dostosowanie typu danych, relacji między tabelami)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja typu dla PricePerDay
            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasColumnType("decimal(18, 2)");

            // Konfiguracja relacji między Reservation a Car
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
