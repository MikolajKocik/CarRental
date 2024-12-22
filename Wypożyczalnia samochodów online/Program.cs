using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Services;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracja logowania
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Dodanie usług dla aplikacji
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Konfiguracja plików cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddTransient<EmailService>();

var app = builder.Build();

// Stosowanie migracji i seedowanie danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    // Automatyczne stosowanie migracji
    context.Database.Migrate();

    // Seedowanie ról
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    // Seedowanie danych samochodów
    if (!context.Cars.Any())
    {
        context.Cars.AddRange(
            new Car
            {
                Brand = "Toyota",
                Model = "Corolla",
                PricePerDay = 120,
                IsAvailable = true,
                ImageUrl = "/Images/toyota_corolla.jpg"
            },
            new Car
            {
                Brand = "Volkswagen",
                Model = "Golf",
                PricePerDay = 150,
                IsAvailable = true,
                ImageUrl = "/Images/volkswagen_golf.jpg"
            },
            new Car
            {
                Brand = "Tesla",
                Model = "Model 3",
                PricePerDay = 400,
                IsAvailable = true,
                ImageUrl = "/Images/tesla.jpg"
            },
            new Car
            {
                Brand = "Toyota",
                Model = "Rav4",
                PricePerDay = 200,
                IsAvailable = true,
                ImageUrl = "/Images/toyota_rav4.jpg"
            },
            new Car
            {
                Brand = "BMW",
                Model = "X5",
                PricePerDay = 300,
                IsAvailable = true,
                ImageUrl = "/Images/bmw_x5.jpg"
            },
            new Car
            {
                Brand = "Fiat",
                Model = "Punto",
                PricePerDay = 80,
                IsAvailable = true,
                ImageUrl = "/Images/fiat_punto.jpg"
            },
            new Car
            {
                Brand = "Mercedes",
                Model = "C-Class",
                PricePerDay = 350,
                IsAvailable = true,
                ImageUrl = "/Images/mercedes_c-class.jpg"
            },
            new Car
            {
                Brand = "Ford",
                Model = "Focus",
                PricePerDay = 100,
                IsAvailable = true,
                ImageUrl = "/Images/ford_focus.jpg"
            },
            new Car
            {
                Brand = "Audi",
                Model = "A4",
                PricePerDay = 250,
                IsAvailable = true,
                ImageUrl = "/Images/audi_a4.jpg"
            }
        );
        await context.SaveChangesAsync();
    }

    // Dodanie konta administratora 
    if (!context.Users.Any(u => u.Email == "admin@example.com"))
    {
        var adminUser = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true // umożliwia logowanie
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
            Console.WriteLine("Admin account created successfully.");
        }
        else
        {
            Console.WriteLine("Failed to create admin account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Dla środowiska deweloperskiego
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=ConfirmEmail}/{userId?}/{token?}");

app.Run();
