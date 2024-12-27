using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Wypo�yczalnia_samochod�w_online.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // BindProperty sprawia, �e w�a�ciwo�� Input jest wi�zana z danymi formularza
        [BindProperty]
        public InputModel Input { get; set; }

        // Model wej�ciowy, kt�ry przechowuje dane wprowadzone przez u�ytkownika (email, has�o)
        public class InputModel
        {
            [Required(ErrorMessage = "Email jest wymagany.")]
            [EmailAddress(ErrorMessage = "Nieprawid�owy format adresu email.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Has�o jest wymagane.")]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        // Akcja, kt�ra obs�uguje logowanie (POST)
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                // Je�li dane wej�ciowe s� nieprawid�owe
                return Page();
            }

            // Znajd� u�ytkownika po e-mailu
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie znaleziono u�ytkownika.");
                return Page();
            }

            // Spr�buj zalogowa� u�ytkownika
            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Je�li u�ytkownik to admin, przekieruj na stron� admina
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Reports", "Admin");
                }

                // Przekierowanie do strony docelowej
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Twoje konto jest zablokowane.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nieprawid�owe dane logowania.");
            }

            // W przypadku b��du, wy�wietl stron� ponownie z b��dami
            return Page();
        }
    }
}
