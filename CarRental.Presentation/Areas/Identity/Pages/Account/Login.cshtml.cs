using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Presentation.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            Input = new InputModel();
        }

        // BindProperty ensures that the Input property is bound to the form data
        [BindProperty]
        public InputModel Input { get; set; }

        // Input model that holds the data entered by the user (email, password)
        public class InputModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string Email { get; set; } = default!;

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; } = default!;

            public bool RememberMe { get; set; }
        }

        // Action that handles login 
        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                // If the input data is invalid
                return Page();
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return Page();
            }

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // If the user is an admin, redirect to the admin page
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Reports", "Admin");
                }

                // Redirect to the target page
                return LocalRedirect(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login credentials.");
            }

            // In case of error, re-display the page with the errors
            return Page();
        }
    }
}
