using CarRental.Domain.Entities;

namespace CarRental.Presentation.Models;

public class AdminReportsViewModel
{
    public int TotalReservations { get; set; }
    public decimal TotalIncome { get; set; }
    public ICollection<Report> PopularCars { get; set; } = new List<Report>();

    // Not confirmed list of reservations 
    public List<Reservation> NotConfirmedReservations { get; set; } = new List<Reservation>();
}
