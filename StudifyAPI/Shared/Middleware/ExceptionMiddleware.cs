using StudifyAPI.Shared;
using StudifyAPI.Shared.Exceptions;
using System.Net;
using System.Text.Json;

namespace StudifyAPI.Shared.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // proceed to next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            // Map specific exceptions to HTTP status codes
            switch (ex)
            {
                case EmailAlreadyUsedException:
                    statusCode = HttpStatusCode.Conflict;
                    break;
                case InvalidPasswordException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
                case UserNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case TaskNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ResponseDTO<string> {
                Data = null,
                Success = false,
                Message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
