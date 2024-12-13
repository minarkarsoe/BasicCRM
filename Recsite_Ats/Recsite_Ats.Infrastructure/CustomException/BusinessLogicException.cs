namespace Recsite_Ats.Infrastructure.CustomException;
public class BusinessLogicException : Exception
{
    public string ResponseCode { get; }
    public string ResponseMessage { get; }

    public BusinessLogicException(string responseCode, string responseMessage)
        : base(responseMessage) // Pass the responseMessage to the base Exception class
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
    }

    public BusinessLogicException(string responseCode, string responseMessage, Exception innerException)
        : base(responseMessage, innerException)
    {
        ResponseCode = responseCode;
        ResponseMessage = responseMessage;
    }
}
