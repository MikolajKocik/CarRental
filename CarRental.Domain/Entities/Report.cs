namespace CarRental.Domain.Entities
{
    public class Report
    {
        public string Brand { get; set; } = default!;
        public string Model { get; set; } = default!;
        public int ReservationCount { get; set; }
    }
}
