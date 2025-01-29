namespace CarRental.Application.Services;

public static class ReservationValidationHelpers
{
    public static bool IsValidDate(DateTime date)
        => date.TimeOfDay == TimeSpan.Zero;
}
