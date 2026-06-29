using System.Text.Json;
using Bookanizer.REST.Exceptions;

namespace Bookanizer.REST.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (InsufficientReadHistoryException ex)
            {
                _logger.LogInformation(
                    ex,
                    "Recommendation gate hit for {Method} {Path}. TraceId: {TraceId}",
                    context.Request.Method, context.Request.Path, context.TraceIdentifier);
                await ProblemWriter.WriteAsync(context, StatusCodes.Status422UnprocessableEntity, "Not enough reading history.");
            }
            catch (RecommenderUnavailableException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Recommender unavailable for {Method} {Path}. TraceId: {TraceId}",
                    context.Request.Method, context.Request.Path, context.TraceIdentifier);
                await ProblemWriter.WriteAsync(context, StatusCodes.Status503ServiceUnavailable, "Recommender temporarily unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}. TraceId: {TraceId}",
                    context.Request.Method, context.Request.Path, context.TraceIdentifier);

                if (context.Response.HasStarted)
                {
                    // Can't rewrite the response => Just rethrow so the server aborts the connection
                    throw;
                }

                await ProblemWriter.WriteAsync(context, StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.");
            }
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(
            this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
