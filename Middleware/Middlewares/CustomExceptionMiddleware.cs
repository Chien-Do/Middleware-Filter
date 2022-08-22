using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Middleware.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Middleware.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public CustomExceptionMiddleware(RequestDelegate next, ILogger logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            { await _next(httpContext);}
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
        public static IApplicationBuilder UseAddresses(
   this IApplicationBuilder app
)
        {
            RouteBuilder builder = new RouteBuilder(app);

            //AddAddressesRoute(app, builder);

            //app.UseRouter(builder.Build());

            return app;
        }
        private static void AddAddressesRoute(IApplicationBuilder app, RouteBuilder builder)
        {
            builder.MapVerb(
                HttpMethod.Get.Method,
                "users/{userId}/addresses",
                async context =>
                {
                    var routeData = context.GetRouteData();
                    var userId = routeData.Values["userId"];

                    // userId available from here
                }
            );
        }
    }



}
