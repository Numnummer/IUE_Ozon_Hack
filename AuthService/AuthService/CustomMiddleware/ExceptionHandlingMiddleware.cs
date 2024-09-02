using Microsoft.AspNetCore.Mvc.Controllers;

namespace AuthMicroservice.CustomMiddleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                GetLogger(context).LogCritical(e.Message);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync(e.Message);
            }
        }

        /// <summary>
        /// Создать логгер для контроллера, исполнявшего запрос
        /// </summary>
        /// <param name="context">Контекст запроса</param>
        /// <returns>Логгер</returns>
        private ILogger GetLogger(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var controllerActionDescriptor = endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();
            var controllerType = controllerActionDescriptor?.ControllerTypeInfo?.AsType();
            return controllerType != null
                ? loggerFactory.CreateLogger(controllerType)
                : loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
        }
    }
}
