using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Middleware.Enums;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Middleware.Filters
{
    public class HasPermissionFilter : AuthorizeAttribute, IAsyncAuthorizationFilter, IActionFilter
    {
        private readonly string[] _permissions;
        private readonly string _displayPermissions;
        private readonly string _entityName;
        private readonly Type _type;

        public HasPermissionFilter(string entityName, Type type, params string[] permissions)
        {
            _entityName = entityName;
            _permissions = permissions;
            _type = type;
            _displayPermissions = $"Error: permission [{ string.Join(",", _permissions)}] required";
        }

        public HasPermissionFilter(Type type, params string[] permissions)
        {
            _entityName = string.Empty;
            _permissions = permissions;
            _type = type;
            _displayPermissions = $"Error: permission [{ string.Join(",", _permissions)}] required";
        }

        public HasPermissionFilter(params string[] permissions)
        {
            _entityName = string.Empty;
            _permissions = permissions;
            _displayPermissions = $"Error: permission [{ string.Join(",", _permissions)}] required";
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var routeData = context.RouteData.Values.ToDictionary(x => x.Key, y => y.Value);
            var data = await CheckPermission.HasPermission(context.HttpContext, _permissions, _entityName, routeData, _type);

            if (data == PermissionEnum.Authorized)
            {
                return;
            }

            if (data == PermissionEnum.Unauthorized)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (data == PermissionEnum.Forbidden)
            {
                context.Result = new JsonResult(_displayPermissions) { StatusCode = 403 };
                return;
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode == HttpStatusCode.Unauthorized.GetHashCode())
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (context.HttpContext.Items["EntityPermissionCalled"] is bool entityCalled && !entityCalled)
            {
                throw new Exception("EntityPermission check not called");
            }


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do nothing
        }
    }
}
