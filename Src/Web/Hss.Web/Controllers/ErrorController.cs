namespace Hss.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        public IActionResult HttpError(int statusCode)
            => statusCode switch
            {
                404 => this.View("Error404"),
                _ => this.View("Error"),
            };
    }
}
