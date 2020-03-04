namespace Hss.Web.Filters
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateModelAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var action = context.ActionDescriptor as ControllerActionDescriptor;
                var controller = context.Controller as Controller;

                context.Result = (IActionResult)action?.ControllerTypeInfo.InvokeMember(
                    action.ActionName,
                    BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    controller,
                    context.ActionDescriptor.BoundProperties.ToArray())
                    ?? new BadRequestResult();

                await context.Result.ExecuteResultAsync(context);
            }

            await next();
        }
    }
}
