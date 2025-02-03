namespace CarRental.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public string CarName { get; set; } = default!;
    public int ReservationsCount { get; set; }
    public decimal TotalIncome { get; set; }
}