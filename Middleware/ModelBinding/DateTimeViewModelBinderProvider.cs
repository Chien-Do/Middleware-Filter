using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Middleware.ModelBinding
{
    public class DateTimeViewModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(DateTimeViewModel))
                return new DateTimeViewModelBinder();
            return null;
        }
    }
}
