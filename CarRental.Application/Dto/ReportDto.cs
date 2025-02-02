namespace CarRental.Application.Dto;

public class ReportDto
{
    public string CarName { get; set; } = default!;
    public int ReservationsCount { get; set; }
    public decimal TotalIncome { get; set; }
}
