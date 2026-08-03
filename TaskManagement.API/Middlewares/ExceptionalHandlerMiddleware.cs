using System.Net;

namespace TaskManagement.API.Middlewares
{
    public class ExceptionalHandlerMiddleware
    {
        private readonly ILogger<ExceptionalHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionalHandlerMiddleware(ILogger<ExceptionalHandlerMiddleware> logger, 
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                logger.LogError(ex, $"{errorId} : {ex.Message}" );
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var errorResponse = new { Id = errorId, message = "An unexpected error occurred." };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
