using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    // Ograniczenie dostępu do tego kontrolera tylko dla zalogowanych użytkowników
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Akcja logowania - metoda POST do wylogowania użytkownika
        [HttpPost]
        [ValidateAntiForgeryToken] // Zapobieganie atakom CSRF
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  // Wylogowanie użytkownika
            return RedirectToAction("Index", "Home");  // Przekierowanie użytkownika na stronę główną po wylogowaniu
        }
    }
}
