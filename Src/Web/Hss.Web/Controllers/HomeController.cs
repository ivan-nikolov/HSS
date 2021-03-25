namespace Hss.Web.Controllers
{
    using System.Diagnostics;

    using Hss.Services.Messaging;
    using Hss.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IEmailSender emailSender;

        public HomeController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [Route("/")]
        [Route("/Home/Index/{id:int}")]
        public IActionResult Index(int id)
        {
            return this.View(id);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Blazor()
        {
            return this.View("_Host");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
