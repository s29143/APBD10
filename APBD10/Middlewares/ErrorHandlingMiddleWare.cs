using System.Net;

namespace APBD10.Middlewares;

public class ErrorHandlingMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleWare> _logger;

    public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unhandled exception occurred");
            await HandleError(context, e);
        }
    }

    private Task HandleError(HttpContext context, Exception e)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var err = new
        {
            message = "Exception occured",
            details = e.Message
        };
        var response = System.Text.Json.JsonSerializer.Serialize(err);

        return context.Response.WriteAsync(response);
    }
}