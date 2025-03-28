﻿using CarRental.Application.Dto;
using CarRental.Domain.Entities;

namespace CarRental.Presentation.Models;

public class AdminReportsViewModel
{
    public int TotalReservations { get; set; }
    public decimal TotalIncome { get; set; }
    public IEnumerable<ReportDto> PopularCars { get; set; } = new List<ReportDto>();
    public IEnumerable<ReservationDto> NotConfirmedReservations { get; set; } = new List<ReservationDto>();
    public IEnumerable<ReservationDto> ConfirmedReservations { get; set; } = new List<ReservationDto>();
}
