namespace CarRental.Application.Services;

public static class CarValidationHelpers
{
    public static bool IsValidUrl(string url)
    {
        var isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        return isValid;
    }

    public static bool IsValidYear(int date)
        => date < 2010 || date > 2025 ? true : false;
}
