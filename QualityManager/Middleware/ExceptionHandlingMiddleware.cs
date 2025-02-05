using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace QualityManager.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");

                httpContext.Response.ContentType = "application/json";

                var response = ex switch
                {
                    ValidationException => new ErrorResponse(HttpStatusCode.BadRequest, "Validation failed. Please check your input.", ex.Message),
                    UnauthorizedAccessException => new ErrorResponse(HttpStatusCode.Unauthorized, "Authentication failed. Please log in.", ex.Message),
                    _ => new ErrorResponse(HttpStatusCode.InternalServerError, "An unexpected error occurred.", ex.Message)
                };

                httpContext.Response.StatusCode = (int)response.StatusCode;
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private record ErrorResponse(HttpStatusCode StatusCode, string Error, string? Details);
    }
}
