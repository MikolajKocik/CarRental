namespace CarRental.Application.Services;

public static class CarValidationYear
{
    public static bool IsValidYear(int date)
        => date < 2010 || date > 2025 ? true : false;
}
