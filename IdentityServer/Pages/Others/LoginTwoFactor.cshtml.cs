using IdentityServerAspNetIdentity.Services;
using IdentityServerHost.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
//using WebApp.Data.Account;


namespace WebApp.Pages.Account
{
    public class LoginTwoFactorModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public EmailMFA EmailMFA { get; set; }

        public LoginTwoFactorModel(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.EmailMFA = new EmailMFA();
        }

        public async Task OnGetAsync(string email, bool rememberMe)
        {
            var user = await userManager.FindByEmailAsync(email);

            this.EmailMFA.SecurityCode = string.Empty;
            this.EmailMFA.RememberMe = rememberMe;

            // Generate the code
            var securityCode = await userManager.GenerateTwoFactorTokenAsync(user, "Email");

            // Send to the user
            await emailService.SendAsync("frankliu.associates@gmail.com",
                email,
                "My Web App's OTP",
                $"Please use this code as the OTP: {securityCode}");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await signInManager.TwoFactorSignInAsync("Email",
                this.EmailMFA.SecurityCode,
                this.EmailMFA.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login2FA", "You are locked out.");
                }
                else
                {
                    ModelState.AddModelError("Login2FA", "Failed to login.");
                }

                return Page();
            }
        }
    }

    public class EmailMFA
    {
        [Required]
        [Display(Name = "Security Code")]
        public string SecurityCode { get; set; }
        public bool RememberMe { get; set; }
    }
}
