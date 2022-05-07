using Microsoft.AspNetCore.Mvc.Filters;

namespace Middleware.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
