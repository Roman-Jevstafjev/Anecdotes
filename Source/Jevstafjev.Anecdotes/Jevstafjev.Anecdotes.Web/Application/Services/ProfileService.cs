using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Jevstafjev.Anecdotes.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Jevstafjev.Anecdotes.Web.Application.Services
{
    public class ProfileService(UserManager<ApplicationUser> userManager, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole> claimsFactory)
        : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
            if (user is null)
            {
                throw new InvalidOperationException();
            }

            context.IssuedClaims = (await claimsFactory.CreateAsync(user)).Claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
            context.IsActive = user != null;
        }
    }
}
