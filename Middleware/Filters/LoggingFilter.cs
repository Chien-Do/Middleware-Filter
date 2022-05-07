using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Middleware.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private ILogger _logger;
        public LoggingFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggingFilter>();
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("OnActionExecuting");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("OnActionExecuted");
        }
    }

}
