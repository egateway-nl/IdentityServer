using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebClientSample.DataAccess;
using WebClientSample.Model;

namespace WebClientSample.Pages.ManageUser
{
    [Authorize]
    public class ChangeDriverModel : PageModel
    {
        private readonly IdpContext _context;

        public ChangeDriverModel(IdpContext context) =>
            _context = context;

        [BindProperty]
        public string? DriverName { get; set; }

        [BindProperty]
        public string? UserId { get; set; }

        //internal List<User>? Users { get; set; }

        internal User? CurrentUser { get; set; }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            UserId = HttpContext.User.Claims.Single(y => y.Type == "UserId").Value;


            //CurrentUser = await _context.AspNetUsers!.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            var claim = await _context.AspNetUserClaims!.SingleOrDefaultAsync(x => x.UserId == UserId && x.ClaimType == "myDriverClaim", cancellationToken);

            DriverName = claim?.ClaimValue;
        }

        public async Task<IActionResult> OnPost(CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var claim = await _context.AspNetUserClaims!.SingleOrDefaultAsync(x => x.UserId == UserId && x.ClaimType == "myDriverClaim", cancellationToken);

                if (claim is null)
                {
                    var newClaim = new Claim(

                        );
                    newClaim.UserId = UserId;
                    newClaim.ClaimType = "myDriverClaim";
                    newClaim.ClaimValue = DriverName;

                    _context.AspNetUserClaims!.Add(newClaim);
                    await _context.SaveChangesAsync(cancellationToken);

                }
                else
                {
                    claim.ClaimValue = DriverName;

                    _context.AspNetUserClaims!.Update(claim);
                    await _context.SaveChangesAsync(cancellationToken);
                }

            }

            // something went wrong, show form with error
            return Page();
        }


    }
}
