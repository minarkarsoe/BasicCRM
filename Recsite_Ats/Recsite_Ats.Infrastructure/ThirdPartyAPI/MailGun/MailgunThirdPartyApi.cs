//namespace Recsite_Ats.Infrastructure.ThirdPartyAPI.MailGun;
//public class MailgunThirdPartyApi : IMailgunThirdPartyApi
//{
//    private readonly Domain.DataTransferObject.MailGun _mailGun;
//    public MailgunThirdPartyApi(IOptions<Domain.DataTransferObject.MailGun> mailgun)
//    {
//        _mailGun = mailgun.Value;
//    }
//    public async Task<MailgunDomainDetails> GetDoaminDetails(string domain)
//    {
//        var authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", _mailGun.ApiKey);
//        var options = new RestClientOptions(_mailGun.BaseUrl) { Authenticator = authenticator };
//        RestClient _client = new RestClient(options);
//        var request = new RestRequest($"v4/domains/{domain}", Method.Get);
//        request.AddQueryParameter("domain", domain);
//        var response = await _client.ExecuteAsync(request);

//        if (response.IsSuccessful)
//        {
//            return JsonSerializer.Deserialize<MailgunDomainDetails>(response.Content);
//        }
//        throw new BusinessLogicException("400", $"Failed to get domain details: {JsonSerializer.Deserialize<MailGunErrorRespnse>(response.Content)}");

//    }
//}
