namespace Hss.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
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

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
