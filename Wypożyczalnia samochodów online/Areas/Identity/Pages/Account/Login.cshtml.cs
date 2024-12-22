using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Wypo¿yczalnia_samochodów_online.Areas.Identity.Pages.Account
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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email jest wymagany.")]
            [EmailAddress(ErrorMessage = "Nieprawid³owy format adresu email.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Has³o jest wymagane.")]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                // Jeœli dane wejœciowe s¹ nieprawid³owe
                return Page();
            }

            // ZnajdŸ u¿ytkownika po e-mailu
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Nie znaleziono u¿ytkownika.");
                return Page();
            }

            // Spróbuj zalogowaæ u¿ytkownika
            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Jeœli u¿ytkownik to admin, przekieruj na stronê admina
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
                ModelState.AddModelError(string.Empty, "Nieprawid³owe dane logowania.");
            }

            return Page();
        }
    }
}
