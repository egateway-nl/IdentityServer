// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer;

// Add profile data for application users by adding properties to the ApplicationUser class
public class CustomProfileService : ProfileService<ApplicationUser>
{
	public CustomProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
	{
	}

	protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
	{
		var principal = await GetUserClaimsAsync(user);
		//var id = (ClaimsIdentity)principal.Identity;
		//if (!string.IsNullOrEmpty(user.Driver))
		//{
		//	id.AddClaim(new Claim("driver", user.Driver));
		//}

		context.AddRequestedClaims(principal.Claims);

		context.IssuedClaims.Add(new System.Security.Claims.Claim("Username", user.UserName));

		// Temp
		context.IssuedClaims.Add(new System.Security.Claims.Claim("UserId", user.Id));
	}
}