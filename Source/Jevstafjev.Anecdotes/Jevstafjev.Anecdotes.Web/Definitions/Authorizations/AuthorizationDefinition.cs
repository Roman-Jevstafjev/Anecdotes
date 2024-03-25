using IdentityModel;
using Jevstafjev.Anecdotes.Domain;
using Jevstafjev.Anecdotes.Web.Definitions.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace Jevstafjev.Anecdotes.Web.Definitions.Authorizations
{
    public class AuthorizationDefinition : AppDefinition
    {
        public override void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = "/account/login";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, "Bearer", options =>
                {
                    options.SaveToken = true;
                    options.Authority = "https://localhost:10001";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        ValidateAudience = false
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            if (string.IsNullOrEmpty(context.Error))
                            {
                                context.Error = "invalid_token";
                            }

                            if (string.IsNullOrEmpty(context.ErrorDescription))
                            {
                                context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                            }

                            if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                                context.Response.Headers.Append("x-token-expired", authenticationException?.Expires.ToString("o"));
                                context.ErrorDescription = $"The token expired on {authenticationException?.Expires:o}";
                            }

                            return context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = context.Error,
                                error_description = context.ErrorDescription
                            }));
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AppData.DefaultPolicyName, x =>
                {
                    x.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
                    x.RequireAuthenticatedUser();
                });
            });
        }

        public override void ConfigureApplication(WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
