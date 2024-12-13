namespace Recsite_Ats.Domain.DataTransferObject;
public class PasswordResetEmailDTO
{
    public string UserName { get; set; }
    public string ResetLink { get; set; }
}
