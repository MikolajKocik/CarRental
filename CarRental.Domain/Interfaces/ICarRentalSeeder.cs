namespace CarRental.Domain.Interfaces
{
    public interface ICarRentalSeeder
    {
        Task SeedIdentityAsync();

        Task SeedCarAsync();

        Task MigrateDatabase();
    }
}
