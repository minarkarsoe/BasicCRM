namespace Recsite_Ats.Domain.ViewModels;

public class ManageTwoFactorViewModel
{
    public bool HasAuthenticator { get; set; }
    public bool Is2faEnabled { get; set; }
    public int RecoveryCodesLeft { get; set; }
}