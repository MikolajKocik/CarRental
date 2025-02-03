using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Presentation.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email address format.")]
            public string Email { get; set; } = default!;

            [Required(ErrorMessage = "Password is required.")]
            [StringLength(100, ErrorMessage = "The password must be at least {2} and at most {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = default!;

            [Required(ErrorMessage = "Password confirmation is required.")]
            [Compare("Password", ErrorMessage = "Passwords must match.")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; } = default!;
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
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    // Sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                // Handle registration errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If the model is invalid, re-display the form
            return Page();
        }
    }
}
