using Microsoft.Extensions.Options;
using RazorLight;
using Recsite_Ats.Application.Common.Interface.Repository;
using Recsite_Ats.Application.Common.Interface.Services;
using Recsite_Ats.Domain.API.APIRequest;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Domain.DataTransferObject.UserDTO;
using Recsite_Ats.Domain.Entites;
using Recsite_Ats.Domain.ViewModels;
using Recsite_Ats.Infrastructure.CustomException;
using RestSharp;
using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace Recsite_Ats.Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly SmtpSettingsDTO _smtpSettings;
    private readonly MailGun _mailGun;
    private readonly IRazorLightEngine _razorLightEngine;
    private readonly RestClient _client;
    private readonly IUnitOfWork _unitOfWork;
    public EmailSender(IOptions<SmtpSettingsDTO> smtpSettings, IOptions<MailGun> mailgun, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _smtpSettings = smtpSettings.Value;
        _mailGun = mailgun.Value;
        _razorLightEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplate"))
            .UseMemoryCachingProvider()
            .Build();
        var authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", _mailGun.ApiKey);
        var options = new RestClientOptions(_mailGun.BaseUrl) { Authenticator = authenticator };
        _client = new RestClient(options);
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpSettings.From),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        return client.SendMailAsync(mailMessage);
    }

    public async Task SendPasswordResetEmailAsync(string email, string userName, string resetLink)
    {
        var templateModel = new PasswordResetEmailDTO
        {
            UserName = userName,
            ResetLink = resetLink
        };

        string template = await _razorLightEngine.CompileRenderAsync("PasswordRestEmailTemplate", templateModel);

        await SendEmailAsync(email, "Reset Password", template);
    }

    public async Task Send2FARecovryEmailAsync(string email, string code)
    {
        var templateModel = new Reset2FAViewModel
        {
            Code = code
        };
        string template = await _razorLightEngine.CompileRenderAsync("RecoveryCodeTemplate", templateModel);
        await SendEmailAsync(email, "Recover 2FA", template);
    }

    public async Task<MailgunDomainDetails> GetDoaminDetails(string domain)
    {
        var request = new RestRequest($"v4/domains/{domain}", Method.Get);
        request.AddQueryParameter("domain", domain);
        var response = await _client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return JsonSerializer.Deserialize<MailgunDomainDetails>(response.Content);
        }
        throw new BusinessLogicException("400", $"Failed to get domain details: {response.ErrorMessage}");

    }
    public async Task<GetDomainKeyResponse> GetDomainKey(GetDomainKeyRequest request)
    {

        var restRequest = new RestRequest($"v1/keys", Method.Get);
        restRequest.AddQueryParameter("domain_name", request.domain_name);
        if (!string.IsNullOrEmpty(request.kind))
        {
            restRequest.AddQueryParameter("kind", request.kind);
        }
        var response = await _client.ExecuteAsync(restRequest);

        if (response.IsSuccessful)
        {
            return JsonSerializer.Deserialize<GetDomainKeyResponse>(response.Content);
        }
        throw new BusinessLogicException("400", $"Failed to get domain details: {response.ErrorMessage}");
    }
    public async Task<AddDomainKeyResponse> AddDomainKey(AddDomainKeyRequest request, ClaimTypesDto userInfo)
    {
        var restRequest = new RestRequest($"v1/keys", Method.Post);
        restRequest.AddQueryParameter("role", request.role);
        if (!string.IsNullOrEmpty(request.kind))
        {
            restRequest.AddQueryParameter("kind", request.kind);
        }
        if (!string.IsNullOrEmpty(request.domain_name))
        {
            restRequest.AddQueryParameter("domain_name", request.domain_name);
        }
        var response = await _client.ExecuteAsync(restRequest);

        if (response.IsSuccessful)
        {
            var result = JsonSerializer.Deserialize<AddDomainKeyResponse>(response.Content);
            MailGunSetting mailGunSetting = new MailGunSetting()
            {
                AccountId = userInfo.AccountId,
                DomainName = request.domain_name,
                ApiKey = result?.key.secret,

            };
            await _unitOfWork.MailGunSetting.Add(mailGunSetting);
            await _unitOfWork.Save();
            return result;
        }
        throw new BusinessLogicException("400", $"Failed to get domain details: {JsonSerializer.Deserialize<MailGunErrorRespnse>(response.Content)?.message}");
    }
    public async Task<MailgunDomainDetails> AddDomain(string domain)
    {
        var authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", _mailGun.ApiKey);
        var options = new RestClientOptions(_mailGun.BaseUrl) { Authenticator = authenticator };
        RestClient client = new RestClient(options);
        var request = new RestRequest("v4/domains", Method.Post);
        request.AddQueryParameter("name", domain);
        var response = await client.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            return JsonSerializer.Deserialize<MailgunDomainDetails>(response.Content);
        }
        throw new BusinessLogicException("400", $"Failed to add domain : {JsonSerializer.Deserialize<MailGunErrorRespnse>(response.Content)?.message} ");
    }
    public async Task<MailgunDomain> VerifyDomain(string domain)
    {
        var request = new RestRequest($"v4/domains/{domain}/verify", Method.Post);
        request.AddJsonBody(new { name = domain });

        var response = await _client.ExecuteAsync(request);
        if (response.IsSuccessful)
        {
            return JsonSerializer.Deserialize<MailgunDomain>(response.Content);
        }
        throw new BusinessLogicException("400", $"Failed to get domain details: {response.ErrorMessage}");
    }

    public async Task SendMessage(string to, string subject, string body)
    {
        var request = new RestRequest($"v3/mail.techhexamyanmar.com/messages", Method.Post);
        request.AddParameter("from", "info.techhexagon@gmail.com");
        request.AddParameter("to[0]", "minarkarsoe@gmail.com");
        request.AddParameter("subject", subject);
        request.AddParameter("html", body);
        var response = await _client.ExecuteAsync(request);
        Console.WriteLine("Response", response);
    }
}