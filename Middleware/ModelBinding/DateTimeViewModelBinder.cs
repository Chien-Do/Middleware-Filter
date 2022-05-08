using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Middleware.ModelBinding
{
    public class DateTimeViewModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
			var day = 0;
			var month = 0;
			var year = 0;
			if (!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["day"], out day) ||
			!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["month"], out month) ||
			!int.TryParse(bindingContext.ActionContext.HttpContext.Request.Query["year"], out year))
			{
				return Task.CompletedTask;
			}
			var result = new DateTimeViewModel
			{
				MyDate = new DateTime(year, month, day)
			};
			bindingContext.Result = ModelBindingResult.Success(result);
			return Task.CompletedTask;
		}
    }
}
