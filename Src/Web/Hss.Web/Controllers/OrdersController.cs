namespace Hss.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : BaseController
    {
        [Authorize]
        public IActionResult BookService()
        {
            return this.View();
        }
    }
}
