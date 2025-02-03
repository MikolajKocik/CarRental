namespace CarRental.Domain.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}
