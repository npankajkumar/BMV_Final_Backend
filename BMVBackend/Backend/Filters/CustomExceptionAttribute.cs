using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Threading;

namespace Backend.Filters
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        public void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (context is SqlException)
            {

            }
        }
    }
}
