namespace CarRental.Domain.Entities;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public decimal PricePerDay { get; set; } 
    public bool IsAvailable { get; set; } 
    public string ImageUrl { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Engine { get; set; } = default!;
    public int Year { get; set; }
    public int ReservationCount { get; set; } = 0;


    // Navigation to reservation
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
