using IdentityModel;
using Jevstafjev.Anecdotes.Infrastructure;
using Jevstafjev.Anecdotes.Web.Application.Services;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using Microsoft.AspNetCore.Identity;

namespace Jevstafjev.Anecdotes.Web.Definitions.IdentityServer
{
    public class IdentityServerDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
            })
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryClients(IdentityServerConfiguration.GetClients())
                .AddInMemoryApiResources(IdentityServerConfiguration.GetApiResources())
                .AddInMemoryIdentityResources(IdentityServerConfiguration.GetIdentityResources())
                .AddInMemoryApiScopes(IdentityServerConfiguration.GetScopes())
                .AddProfileService<ProfileService>()
                .AddDeveloperSigningCredential();
        }

        public override void ConfigureApplication(WebApplication app)
        {
            app.UseIdentityServer();
        }
    }
}
