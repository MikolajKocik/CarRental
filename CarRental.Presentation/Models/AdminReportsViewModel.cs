using CarRental.Domain.Entities;

namespace CarRental.Presentation.Models;

public class AdminReportsViewModel
{
    public int TotalReservations { get; set; }
    public decimal TotalIncome { get; set; }
    public List<CarReport> PopularCars { get; set; } = new List<CarReport>();

    // Not confirmed list of reservations 
    public List<Reservation> NotConfirmedReservations { get; set; } = new List<Reservation>();
}

// Car report of single model
public class CarReport
{
    public string Brand { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int ReservationCount { get; set; }  
}
