using System.Text.Json;

namespace Bookanizer.REST.Middleware
{
    public static class ProblemWriter
    {
        public static async Task WriteAsync(
            HttpContext context, int statusCode, string detail)
        {
            var problem = new
            {
                type = $"https://httpstatuses.io/{statusCode}",
                title = ReasonFor(statusCode),
                status = statusCode,
                detail,
                traceId = context.TraceIdentifier
            };

            if (context.Response.HasStarted)
                return;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }

        public static string ReasonFor(int statusCode) => statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad request.",
            StatusCodes.Status401Unauthorized => "Unauthorized.",
            StatusCodes.Status403Forbidden => "Forbidden.",
            StatusCodes.Status404NotFound => "Resource not found.",
            StatusCodes.Status405MethodNotAllowed => "Method not allowed.",
            StatusCodes.Status422UnprocessableEntity => "Unprocessable request.",
            StatusCodes.Status503ServiceUnavailable => "Service unavailable.",
            StatusCodes.Status500InternalServerError => "Internal server error.",
            _ => "Error."
        };

        // Below is a DetailFor method for a more use of the RFC 9457 shape: Details can be provided this way
        // (would need replacement of ReasonFor in EXCEPTION HANDLING in Program.cs to actually be called)
        //public static string DetailFor(int statusCode) => statusCode switch
        //{
        //    StatusCodes.Status400BadRequest => "Bad request.",
        //    StatusCodes.Status401Unauthorized => "Unauthorized.",
        //    StatusCodes.Status403Forbidden => "Forbidden.",
        //    StatusCodes.Status404NotFound => "Resource not found.",
        //    StatusCodes.Status405MethodNotAllowed => "Method not allowed.",
        //    StatusCodes.Status422UnprocessableEntity => "Unprocessable request.",
        //    StatusCodes.Status503ServiceUnavailable => "Service unavailable.",
        //    StatusCodes.Status500InternalServerError => "Internal server error.",
        //    _ => "Error."
        //};
    }
}