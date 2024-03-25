using Jevstafjev.Anecdotes.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jevstafjev.Anecdotes.Web.Pages.Account
{
    public class LoginModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : PageModel
    {
        public void OnGet() => Input = new LoginViewModel
        {
            ReturnUrl = ReturnUrl
        };

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await userManager.FindByNameAsync(Input.UserName);
            if (user is null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return Page();
            }

            var result = await signInManager.PasswordSignInAsync(user, Input.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return Page();
            }

            return Redirect(Input.ReturnUrl);
        }

        [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = null!;

        [BindProperty] public LoginViewModel Input { get; set; } = null!;
    }
}
