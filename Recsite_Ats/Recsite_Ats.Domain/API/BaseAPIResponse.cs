namespace Recsite_Ats.Domain.API;
public class BaseAPIResponse
{
    public string RespCode { get; set; }
    public string RespDescription { get; set; }
    public string Mode { get; set; }
    public string Version { get; set; }
    public object Data { get; set; }

    public BaseAPIResponse(object data, string message, string code, string mode = "dev")
    {
        RespCode = code;
        RespDescription = message;
        Data = data;
        Mode = mode;
        Version = "1.0";
    }
}
