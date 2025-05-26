namespace CarRental.Domain.Interfaces.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
}
