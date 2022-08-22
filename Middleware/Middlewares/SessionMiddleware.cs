using Microsoft.AspNetCore.Http;
using Middleware.Services;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;
        public SessionMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;   
            _userService = userService; 

        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                await _next(httpContext);
                return;
            }
            Dictionary<string, string[]> userSession = new Dictionary<string, string[]>();
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(httpContext.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                var user = await _userService.GetUser(username, password);

                userSession.Add("Permission", user.Permissions);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = 401;
                return;
            }

            httpContext.Items.Add("Session", userSession);
            await _next(httpContext);

        }
    }
}
