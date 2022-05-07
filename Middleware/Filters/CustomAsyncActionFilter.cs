using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Middleware.Filters
{
    public class CustomAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // before the action executes  
            await next();
            // after the action executes  
        }
    }
}
