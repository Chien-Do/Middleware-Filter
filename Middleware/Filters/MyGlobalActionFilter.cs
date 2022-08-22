using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Middleware.Attributes;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Middleware.Filters
{
    public class MyGlobalActionFilter : IActionFilter
    {

        private ILogger _logger;
        public MyGlobalActionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggingFilter>();
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes()
                                                          .Any(a => a.GetType().Equals(typeof(IgnoreStatusAttribute)));

                if (!isDefined)
                {
                    //Write code apply global
                }
            }

            _logger.LogInformation($" Global - {MethodBase.GetCurrentMethod()}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($" Global - {MethodBase.GetCurrentMethod()}");
        }
    }
}
