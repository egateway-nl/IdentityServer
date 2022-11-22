//using WebApp.Data.Account;

namespace IdentityServer.Pages.Others;

[Authorize]
public class AuthenticatorWithMFASetupModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;

    [BindProperty]
    public SetupMFAViewModel ViewModel { get; set; }

    [BindProperty]
    public bool Succeeded { get; set; }

    public AuthenticatorWithMFASetupModel(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
        ViewModel = new SetupMFAViewModel();
        Succeeded = false;
    }

    public async Task OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);
        await userManager.ResetAuthenticatorKeyAsync(user);
        var key = await userManager.GetAuthenticatorKeyAsync(user);
        ViewModel.Key = key;
        ViewModel.QRCodeBytes = GenerateQRCodeBytes("my web app", key, user.Email);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await userManager.GetUserAsync(User);
        if (await userManager.VerifyTwoFactorTokenAsync(
            user,
            userManager.Options.Tokens.AuthenticatorTokenProvider,
            ViewModel.SecurityCode))
        {
            await userManager.SetTwoFactorEnabledAsync(user, true);
            Succeeded = true;
        }
        else
        {
            ModelState.AddModelError("AuthenticatorSetup", "Some went wrong with authenticator setup.");
        }

        return Page();
    }

    private byte[] GenerateQRCodeBytes(string provider, string key, string userEmail)
    {
        var qrCodeGenerater = new QRCodeGenerator();
        var qrCodeData = qrCodeGenerater.CreateQrCode(
            $"otpauth://totp/{provider}:{userEmail}?secret={key}&issuer={provider}",
            QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);
        var qrCodeImage = qrCode.GetGraphic(20);

        return BitmapToByteArray(qrCodeImage);
    }

    private byte[] BitmapToByteArray(Bitmap image)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}

public class SetupMFAViewModel
{
    public string Key { get; set; }

    [Required]
    [Display(Name = "Code")]
    public string SecurityCode { get; set; }

    public byte[] QRCodeBytes { get; set; }
}

