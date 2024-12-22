using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Wypożyczalnia_samochodów_online.Services;

namespace Wypożyczalnia_samochodów_online.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly EmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email jest wymagany.")]
            [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Hasło jest wymagane.")]
            [StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} i maksymalnie {1} znaków.", MinimumLength = 6)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
            [Compare("Password", ErrorMessage = "Hasła muszą się zgadzać.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    EmailConfirmed = false // Domyślnie niepotwierdzony
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Generowanie tokena potwierdzenia e-mail
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Tworzenie linku do potwierdzenia
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);

                    // Wysyłanie wiadomości e-mail z potwierdzeniem
                    await _emailService.SendEmailAsync(
                        Input.Email,
                        "Potwierdzenie rejestracji",
                        $"Kliknij w poniższy link, aby potwierdzić rejestrację: <a href='{confirmationLink}'>Potwierdź</a>"
                    );

                    // Logowanie użytkownika (opcjonalne) lub przekierowanie
                    return RedirectToAction("Index", "Home");
                }

                // Obsługa błędów rejestracji
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
