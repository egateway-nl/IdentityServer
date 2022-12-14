using System.Threading.Tasks;
//using WebApp.Data.Account;

namespace WebApp.Pages.Account
{
    public class LoginTwoFactorWithAuthenticatorModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public AuthenticatorMFA AuthenticatorMFA { get; set; }

        public LoginTwoFactorWithAuthenticatorModel(SignInManager<ApplicationUser> signInManager)
        {
            this.AuthenticatorMFA = new AuthenticatorMFA();
            this.signInManager = signInManager;
        }

        public void OnGet(bool rememberMe)
        {
            this.AuthenticatorMFA.SecurityCode = string.Empty;
            this.AuthenticatorMFA.RememberMe = rememberMe;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(
                this.AuthenticatorMFA.SecurityCode,
                this.AuthenticatorMFA.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Authenticator2FA", "You are locked out.");
                }
                else
                {
                    ModelState.AddModelError("Authenticator2FA", "Failed to login.");
                }

                return Page();
            }
        }
    }

    public class AuthenticatorMFA
    {
        [Required]
        [Display(Name = "Code")]
        public string SecurityCode { get; set; }

        public bool RememberMe { get; set; }
    }
}
