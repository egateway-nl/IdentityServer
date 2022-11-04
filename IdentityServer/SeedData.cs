using IdentityModel;
using IdentityServerAspNetIdentity.Data;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var payman = userMgr.FindByNameAsync("payman").Result;
            if (payman == null)
            {
                payman = new ApplicationUser
                {
                    UserName = "payman",
                    Email = "paymantatar@email.com",
                    EmailConfirmed = true,
                    Driver = "8888",
                };
                var result = userMgr.CreateAsync(payman, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(payman, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "payman tatar"),
                            new Claim(JwtClaimTypes.GivenName, "payman"),
                            new Claim(JwtClaimTypes.FamilyName, "tatar"),
                            new Claim(JwtClaimTypes.WebSite, "http://payman.com"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("payman created");
            }
            else
            {
                Log.Debug("payman already exists");
            }

            var testuser = userMgr.FindByNameAsync("testuser").Result;
            if (testuser == null)
            {
                testuser = new ApplicationUser
                {
                    UserName = "testuser",
                    Email = "testuser@email.com",
                    EmailConfirmed = true,
                    Driver = "123456",
                };
                var result = userMgr.CreateAsync(testuser, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(testuser, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "testuser ta"),
                            new Claim(JwtClaimTypes.GivenName, "testuser"),
                            new Claim(JwtClaimTypes.FamilyName, "ta"),
                            new Claim(JwtClaimTypes.WebSite, "http://testuser.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("testuser created");
            }
            else
            {
                Log.Debug("testuser already exists");
            }
        }
    }
}
