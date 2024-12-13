using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Recsite_Ats.Domain.API;
using Recsite_Ats.Domain.DataTransferObject;
using Recsite_Ats.Infrastructure.Data;
using Serilog;
using System.Text;
using System.Text.Json;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            // Capture the request details
            string requestData = await ReadRequestBodyAsync(context.Request);

            try
            {
                // Get the current endpoint for the request
                var endpoint = context.GetEndpoint();
                if (endpoint != null && endpoint.Metadata.OfType<AllowAnonymousAttribute>().Any())
                {
                    // If the endpoint has the [AllowAnonymous] attribute, skip token validation
                    await _next(context);
                    return;
                }

                // If the route doesn't have [AllowAnonymous], check for the token
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    await LogAndRespondAsync(context, "Unauthorized: Missing or invalid token.", "401", requestData);
                    return;
                }

                var token = authHeader["Bearer ".Length..].Trim();

                // Create a scope for resolving ApplicationDbContext
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // Resolve ApplicationDbContext

                    var userToken = await _context.UserTokens
                        .Where(t => t.JwtToken == token)
                        .FirstOrDefaultAsync();

                    if (userToken == null || userToken.IsRevoked || userToken.ExpirationDate < DateTime.Now)
                    {
                        await LogAndRespondAsync(context, "Unauthorized: Token is invalid or revoked.", "401", requestData);
                        return;
                    }
                }

                // Process the request
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, requestData);
            }
            finally
            {
                // Capture response details
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseData = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var controllerName = context.GetRouteValue("controller")?.ToString() ?? "Unknown";
                var actionName = context.GetRouteValue("action")?.ToString() ?? "Unknown";
                var statusCode = context.Response.StatusCode;

                // Log response
                LoggingDTO logger = new LoggingDTO
                {
                    ControllerName = controllerName,
                    ActionName = actionName,
                    Message = statusCode == 200 ? "Success" : "Error",
                    RequestData = requestData,
                    ResponseData = responseData
                };

                if (logger.Message != "Success")
                {
                    Log.Error("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", logger.ControllerName, logger.ActionName, logger.Message, logger.RequestData, logger.ResponseData);
                }
                else
                {
                    Log.Information("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", logger.ControllerName, logger.ActionName, logger.Message, logger.RequestData, logger.ResponseData);
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await streamReader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private async Task LogAndRespondAsync(HttpContext context, string message, string statusCode, string requestData)
    {

        var controllerName = context.GetRouteValue("controller")?.ToString() ?? "Unknown";
        var actionName = context.GetRouteValue("action")?.ToString() ?? "Unknown";

        var log = new LoggingDTO
        {
            ControllerName = controllerName,
            ActionName = actionName,
            Message = message,
            RequestData = requestData,
            ResponseData = JsonSerializer.Serialize(new BaseAPIResponse(null, message, statusCode))
        };

        if (log.Message != "Success")
        {
            Log.Error("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", log.ControllerName, log.ActionName, log.Message, log.RequestData, log.ResponseData);
        }
        else
        {
            Log.Information("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", log.ControllerName, log.ActionName, log.Message, log.RequestData, log.ResponseData);
        }

        context.Response.StatusCode = int.Parse(statusCode);
        context.Response.ContentType = "application/json";
        var result = Encoding.UTF8.GetBytes(log.ResponseData);
        await context.Response.Body.WriteAsync(result, 0, result.Length);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string requestData)
    {
        var controllerName = context.GetRouteValue("controller")?.ToString() ?? "Unknown";
        var actionName = context.GetRouteValue("action")?.ToString() ?? "Unknown";

        var log = new LoggingDTO
        {
            ControllerName = controllerName,
            ActionName = actionName,
            Message = exception.Message,
            RequestData = requestData,
            ResponseData = JsonSerializer.Serialize(new BaseAPIResponse(null, exception.Message, "500"))
        };

        if (log.Message != "Success")
        {
            Log.Error("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", log.ControllerName, log.ActionName, log.Message, log.RequestData, log.ResponseData);
        }
        else
        {
            Log.Information("Execite : Controller : {Controller} , Method : {Method} , Message : {message} , RequestData : {request} , ResponseData : {response}", log.ControllerName, log.ActionName, log.Message, log.RequestData, log.ResponseData);
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var result = Encoding.UTF8.GetBytes(log.ResponseData);
        await context.Response.Body.WriteAsync(result, 0, result.Length);
    }
}
