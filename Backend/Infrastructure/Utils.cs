using System.Net;
using System.Net.Mime;
using Backend.Exceptions;

namespace Backend.Infrastructure;

public static class Utils
{
    public static HttpStatusCode ExceptionToStatusCode(this Exception exception)
        => exception switch
        {
            InvalidInputException => HttpStatusCode.BadRequest,
            ResourceNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

    public static async Task WriteJsonToHttpResponseAsync<TResponse>(HttpResponse httpResponse, HttpStatusCode statusCode, TResponse response)
    {
        httpResponse.ContentType = MediaTypeNames.Application.Json;
        httpResponse.StatusCode = (int)statusCode;
        await httpResponse.WriteAsJsonAsync(response);
    }
}