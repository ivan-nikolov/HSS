namespace Hss.Web.Filters
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelValidationActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var action = context.ActionDescriptor as ControllerActionDescriptor;
                var controller = context.Controller as Controller;

                var isAsyncAction = this.IsAsyncAction(action);
                var result = (isAsyncAction
                    ? await this.InvokeAsyncAction(action, controller, context)
                    : this.InvokeAction(action, controller, context)) as IActionResult;

                context.Result = result ?? new BadRequestResult();

                await context.Result.ExecuteResultAsync(context);
            }

            await next();
        }

        private object InvokeAction(ControllerActionDescriptor action, Controller controller, ActionExecutingContext context)
            => action?.ControllerTypeInfo.InvokeMember(
                    action.ActionName,
                    BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance,
                    null,
                    controller,
                    context.ActionDescriptor.BoundProperties.ToArray());

        private async Task<object> InvokeAsyncAction(ControllerActionDescriptor action, Controller controller, ActionExecutingContext context)
            => await (Task<IActionResult>)this.InvokeAction(action, controller, context);

        private bool IsAsyncAction(ControllerActionDescriptor actionDescriptor)
        {
            Type attType = typeof(AsyncStateMachineAttribute);
            AsyncStateMachineAttribute attribute = actionDescriptor.MethodInfo.GetCustomAttribute(attType) as AsyncStateMachineAttribute;

            return attribute != null;
        }
    }
}
