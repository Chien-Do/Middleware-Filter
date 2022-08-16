using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
            _logger.LogInformation($" Global - {MethodBase.GetCurrentMethod()}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($" Global - {MethodBase.GetCurrentMethod()}");
        }
    }
}
