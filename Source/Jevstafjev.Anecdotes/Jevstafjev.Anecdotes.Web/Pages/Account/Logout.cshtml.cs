using Duende.IdentityServer.Services;
using Jevstafjev.Anecdotes.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jevstafjev.Anecdotes.Web.Pages.Account
{
    public class LogoutModel(IIdentityServerInteractionService interactionService,
        SignInManager<ApplicationUser> signInManager) : PageModel
    {
        public async Task<IActionResult> OnPost()
        {
            var logoutContext = await interactionService.GetLogoutContextAsync(LogoutId);
            await signInManager.SignOutAsync();

            return Redirect(logoutContext.PostLogoutRedirectUri!);
        }

        [BindProperty(SupportsGet = true)] public string LogoutId { get; set; } = null!;
    }
}
