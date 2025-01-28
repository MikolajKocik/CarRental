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

// Konfiguracja Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>() // Rejestracja używanego kontekstu bazy danych
.AddDefaultTokenProviders(); // Dodanie domyślnych dostawców tokenów

// Konfiguracja plików cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Identity/Account/Login";  // Ścieżka do strony logowania
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Ścieżka w przypadku braku uprawnień
    options.SlidingExpiration = true; 
});

builder.Services.AddControllersWithViews(); // Dodanie obsługi kontrolerów i widoków
builder.Services.AddRazorPages();  // Dodanie obsługi Razor Pages
builder.Services.AddTransient<EmailService>(); // Dodanie usługi wysyłania e-maili

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
                ImageUrl = "/Images/toyota_corolla.jpg",
                Year = 2022,
                Engine = "1.6 Benzyna",
                Description = "Niezawodny, miejski sedan o niskim spalaniu i wygodnym wnętrzu."
            },
            new Car
            {
                Brand = "Volkswagen",
                Model = "Golf",
                PricePerDay = 150,
                IsAvailable = true,
                ImageUrl = "/Images/volkswagen_golf.jpg",
                Year = 2021,
                Engine = "1.5 TSI",
                Description = "Popularny kompakt do miasta i na trasę, ceniony za uniwersalność i oszczędność."
            },
            new Car
            {
                Brand = "Tesla",
                Model = "Model 3",
                PricePerDay = 400,
                IsAvailable = true,
                ImageUrl = "/Images/tesla.jpg",
                Year = 2023,
                Engine = "Elektryczny",
                Description = "Elektryczna innowacja z imponującym przyspieszeniem i nowoczesnymi rozwiązaniami."
            },
            new Car
            {
                Brand = "Toyota",
                Model = "Rav4",
                PricePerDay = 200,
                IsAvailable = true,
                ImageUrl = "/Images/toyota_rav4.jpg",
                Year = 2023,
                Engine = "2.0 Hybryda",
                Description = "Wszechstronny SUV z napędem hybrydowym i przestronnym wnętrzem."
            },
            new Car
            {
                Brand = "BMW",
                Model = "X5",
                PricePerDay = 300,
                IsAvailable = true,
                ImageUrl = "/Images/bmw_x5.jpg",
                Year = 2021,
                Engine = "3.0 Diesel",
                Description = "Luksusowy SUV o sportowym charakterze, gwarantuje komfort i wysokie osiągi."
            },
            new Car
            {
                Brand = "Fiat",
                Model = "Punto",
                PricePerDay = 80,
                IsAvailable = true,
                ImageUrl = "/Images/fiat_punto.jpg",
                Year = 2019,
                Engine = "1.2 Benzyna",
                Description = "Niewielkie, ekonomiczne auto idealne do jazdy miejskiej i łatwego parkowania."
            },
            new Car
            {
                Brand = "Mercedes",
                Model = "C-Class",
                PricePerDay = 350,
                IsAvailable = true,
                ImageUrl = "/Images/mercedes_c-class.jpg",
                Year = 2021,
                Engine = "2.0 Diesel",
                Description = "Elegancka limuzyna łącząca luksus i nowoczesne technologie w codziennej jeździe."
            },
            new Car
            {
                Brand = "Ford",
                Model = "Focus",
                PricePerDay = 100,
                IsAvailable = true,
                ImageUrl = "/Images/ford_focus.jpg",
                Year = 2020,
                Engine = "1.0 EcoBoost",
                Description = "Przestronny kompakt z dynamicznym silnikiem, świetnie sprawdza się w mieście i na trasach."
            },
            new Car
            {
                Brand = "Audi",
                Model = "A4",
                PricePerDay = 250,
                IsAvailable = true,
                ImageUrl = "/Images/audi_a4.jpg",
                Year = 2022,
                Engine = "2.0 TFSI",
                Description = "Nowoczesny sedan z mocnym silnikiem i komfortowym wnętrzem, idealny na długie podróże."
            }
        );
        await context.SaveChangesAsync(); // Zapisujemy do bazy
    }

    // Dodanie konta administratora, jeśli jeszcze nie istnieje
    if (!context.Users.Any(u => u.Email == "admin@example.com"))
    {
        var adminUser = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true 
        };
        var result = await userManager.CreateAsync(adminUser, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin"); // Dodanie użytkownika do roli Admin
            Console.WriteLine("Admin account created successfully.");
        }
        else
        {
            Console.WriteLine("Failed to create admin account: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}

// Konfiguracja middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Dla środowiska deweloperskiego
}
else
{
    app.UseExceptionHandler("/Home/Error"); // W przypadku błędów używamy niestandardowej strony
    app.UseHsts(); // Wymusza użycie HTTPS
}

app.UseHttpsRedirection(); // Przekierowuje na HTTPS
app.UseStaticFiles(); // Umożliwia dostęp do plików statycznych (np. obrazków, CSS)

app.UseRouting(); // Konfiguruje routing

app.UseAuthentication(); // Umożliwia uwierzytelnianie
app.UseAuthorization(); // Umożliwia autoryzację

app.MapRazorPages(); // Obsługuje strony Razor

// Definicja domyślnej trasy kontrolera
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ścieżka do potwierdzenia e-maila
app.MapControllerRoute(
    name: "account",
    pattern: "{controller=Account}/{action=ConfirmEmail}/{userId?}/{token?}");

app.Run(); // Uruchomienie aplikacji
