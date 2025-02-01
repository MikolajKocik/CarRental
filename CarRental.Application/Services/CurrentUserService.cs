using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarRental.Application.Services;

/*
 
CurrentUserService allows you to get the current user ID from the HTTP context.
This allows the handler or other components to easily get information
about the user without directly accessing the HttpContext.

*/

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public string? UserId { get; }
}
