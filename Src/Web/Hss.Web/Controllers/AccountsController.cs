namespace Hss.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AccountsController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
