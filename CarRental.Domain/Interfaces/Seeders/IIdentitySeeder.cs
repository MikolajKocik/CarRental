﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CarRental.Domain.Interfaces.Seeders;

public interface IIdentitySeeder
{
    Task SeederIdentityAsync(RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager, ILogger<IdentityUser> logger);
}
