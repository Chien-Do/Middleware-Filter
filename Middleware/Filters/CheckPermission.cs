using Microsoft.AspNetCore.Http;
using Middleware.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Middleware.Filters
{
    public static class CheckPermission
    {
        public static async Task<PermissionEnum> HasPermission(HttpContext httpContext, string[] permissions,
           string entityName, IDictionary<string, object> keyValuesRouteData, Type type)
        {
            if (httpContext.Response.StatusCode == HttpStatusCode.Unauthorized.GetHashCode() || !httpContext.User.Identity.IsAuthenticated)
            {
                return PermissionEnum.Unauthorized;
            }

            var session = (Dictionary<string,string>)httpContext.Items["Session"];

            if (session == null)
            {
                return PermissionEnum.Forbidden;
            }

           

            if (permissions.Length == 0)
            {
                return PermissionEnum.Forbidden;
            }

            //string[] userPermissions = session.AuthorizePermissions;
            if (session.Count  == 0)
            {
                return PermissionEnum.Forbidden;
            }
            

            //var intersect = permissions.Intersect(userPermissions);

            //if (!intersect.Any())
            //{
            //    return PermissionEnum.Forbidden;
            //}

           

            return PermissionEnum.Authorized;
        }
    }
}
