using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Middleware.Attributes
{
    public class SampleActionFilterAttribute : TypeFilterAttribute
    {
        public SampleActionFilterAttribute()
                             : base(typeof(SampleActionFilter)) {}
        private class SampleActionFilter : IActionFilter
        {
            private readonly ILogger _logger;
            public SampleActionFilter(ILoggerFactory loggerFactory)
            {
                _logger = loggerFactory.CreateLogger<SampleActionFilterAttribute>();
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                _logger.LogInformation($" Filter Atrribute - {MethodBase.GetCurrentMethod()}");
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                _logger.LogInformation($" Filter Atrribute - {MethodBase.GetCurrentMethod()}");
            }
        }
    }
}
