namespace Recsite_Ats.Domain.DataTransferObject;
public class LoggingDTO
{
    public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public string Message { get; set; }
    public string? RequestData { get; set; }
    public string ResponseData { get; set; }

}
