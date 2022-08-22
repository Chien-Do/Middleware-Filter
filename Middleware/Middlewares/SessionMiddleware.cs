using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Middleware.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;   

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var header = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(header))
            {
                await _next(httpContext);
                return;
            }
            Dictionary<string, string> userSession = new Dictionary<string, string>();
            try
            {
                userSession.Add("Permission", "Staff");
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            httpContext.Items.Add("Session", userSession);

        }
    }
}
