using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;

namespace Recsite_Ats.Application.Common.Interface.Services;
public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
    Task SendPasswordResetEmailAsync(string email, string userName, string resetLink);
    Task Send2FARecovryEmailAsync(string email, string code);
    Task<MailgunDomainDetails> AddDomain(string domain);
    Task<MailgunDomain> VerifyDomain(string domain);
    Task<MailgunDomainDetails> GetDoaminDetails(string domain);
    Task<GetDomainKeyResponse> GetDomainKey(GetDomainKeyRequest request);
    Task<AddDomainKeyResponse> AddDomainKey(AddDomainKeyRequest request, ClaimTypesDto userInfo);
    Task SendMessage(string to, string subject, string body);
}
