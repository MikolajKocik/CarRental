using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CarRental.Application.Services;

/*
 
CurrentUserService allows you to get the current user ID from the HTTP context.
This allows the handler or other components to easily get information
about the user without directly accessing the HttpContext.

*/

public class CurrentUserService : ICurrentUserService
{
    private readonly ILogger<CurrentUserService> _logger;


    public CurrentUserService(IHttpContextAccessor httpContextAccessor, ILogger<CurrentUserService> logger)
    {
        _logger = logger;

        UserId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        _logger.LogInformation($"Extracted UserId: {UserId}");
    }

    public string? UserId { get; }
}
