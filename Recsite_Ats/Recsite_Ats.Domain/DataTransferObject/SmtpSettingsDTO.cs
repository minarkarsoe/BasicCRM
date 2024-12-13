namespace Recsite_Ats.Domain.DataTransferObject;
public class SmtpSettingsDTO
{
    public string From { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class MailGun
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}


public record MailgunDomainDetails(object domain, DnsRecord[] receiving_dns_records, DnsRecord[] sending_dns_records);
public record DnsRecord(bool is_active, string[] cached, string record_type, string valid, string? name, string value, string? priority);

public record MailgunDomain(
    string created_at,
    string id,
    bool is_disabled,
    string name,
    bool require_tls,
    bool skip_verification,
    string smtp_login,
    string spam_action,
    string state,
    string type,
    bool use_automatic_sender_security,
    string web_prefix,
    string web_scheme,
    bool wildcard,
    DomainDisabled disabled,
    string smtp_password,
    string tarcking_host,
    DnsRecord[] sending_dns_records,
    DnsRecord[] receiving_dns_records);

public record GetDomainKeysItem(string id, string decription, string kind, string role, string created_at, string updated_at, string domain_name, string requestor, string user_name, string expires_at, string secret);
public record GetDomainKeyResponse(int total_count, GetDomainKeysItem[] items);

public record AddDomainKeyResponse(string message, GetDomainKeysItem key);

public record DomainDisabled(string code, string note, bool permantely, string reason, string util);

public record AddDomain(string domain);

public record MailGunErrorRespnse(string message);