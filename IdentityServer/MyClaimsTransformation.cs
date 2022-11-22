//namespace IdentityServer
//{
//	public class MyClaimsTransformation
//	{
//	}
//}
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServerAspNetIdentity;
public class MyClaimsTransformation : IClaimsTransformation
{
	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		ClaimsIdentity claimsIdentity = new ClaimsIdentity();
		var claimType = "myDriverClaim";
		if (!principal.HasClaim(claim => claim.Type == claimType))
		{
			claimsIdentity.AddClaim(new Claim(claimType, "myDriverClaimValue"));
		}

		principal.AddIdentity(claimsIdentity);
		return Task.FromResult(principal);
	}
}
