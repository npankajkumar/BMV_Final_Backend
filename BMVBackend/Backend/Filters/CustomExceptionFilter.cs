using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is IOException)
            {

            }
        }
    }
}
