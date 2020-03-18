namespace Hss.Web.Filters
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ArgumentNullExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(ArgumentNullException))
            {
                context.Result = new RedirectResult("/Home/Error");
            }
        }
    }
}
