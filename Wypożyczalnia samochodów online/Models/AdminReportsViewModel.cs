using System.Collections.Generic;

namespace Wypożyczalnia_samochodów_online.Models
{
    public class AdminReportsViewModel
    {
        public int TotalReservations { get; set; }
        public decimal TotalIncome { get; set; }
        public List<CarReport> PopularCars { get; set; }
    }

    public class CarReport
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReservationCount { get; set; }
    }
}
