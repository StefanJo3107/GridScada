using Backend.Models;

namespace Backend.Infrastructure;

public class ExceptionMiddleware
{
    private readonly bool _isDevelopment;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, bool isDevelopment)
    {
        _next = next;
        _isDevelopment = isDevelopment;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            if (httpContext.RequestAborted.IsCancellationRequested) return;


            ExceptionResponse exceptionResponse = new(exception);
            if (_isDevelopment)
                exceptionResponse.StackTrace = exception.StackTrace;
            Console.WriteLine(exception.ToString());
            var statusCode = exception.ExceptionToStatusCode();
            await Utils.WriteJsonToHttpResponseAsync(httpContext.Response, statusCode, exceptionResponse);
        }
    }
}