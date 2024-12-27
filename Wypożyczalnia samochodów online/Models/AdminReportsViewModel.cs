using System.Collections.Generic;

namespace Wypożyczalnia_samochodów_online.Models
{
    // ViewModel dla raportu administratora
    public class AdminReportsViewModel
    {
        public int TotalReservations { get; set; }
        public decimal TotalIncome { get; set; }
        public List<CarReport> PopularCars { get; set; }

        // Lista rezerwacji niepotwierdzonych 
        public List<Reservation> NotConfirmedReservations { get; set; }
    }

    // Model pojedynczego raportu samochodu
    public class CarReport
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReservationCount { get; set; }  // Liczba rezerwacji, które dotyczyły tego samochodu
    }
}
