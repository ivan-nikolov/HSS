namespace Hss.Web.Controllers
{
    using System.Threading.Tasks;

    using Hss.Services.Data.Services;
    using Hss.Web.ViewModels.Services;
    using Microsoft.AspNetCore.Mvc;

    public class ServicesController : BaseController
    {
        private readonly IServicesService servicesService;

        public ServicesController(IServicesService servicesService)
        {
            this.servicesService = servicesService;
        }

        public async Task<IActionResult> Details(int id)
        {
            var serviceModel = await this.servicesService.GetByIdAsync<DetailsServiceViewModel>(id);
            if (serviceModel == null)
            {
                return this.NotFound();
            }

            return this.View(serviceModel);
        }
    }
}
