namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Data.Categories;
    using Hss.Services.Data.Services;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Administration.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ServicesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IServicesService servicesService;

        public ServicesController(ICategoriesService categoriesService, IServicesService servicesService)
        {
            this.categoriesService = categoriesService;
            this.servicesService = servicesService;
        }

        public IActionResult Create()
        {
            var inputModel = new CreateServiceInputModel()
            {
                Categories = this.categoriesService
                .GetAllRootCategories()
                .Select(c => new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                }),
            };

            return this.View(inputModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Create(CreateServiceInputModel input)
        {
            var category = await this.categoriesService.GetByIdAsync(input.CategoryId);
            var serviceModel = input.To<ServiceServiceModel>();
            await this.servicesService.CreateAsync(serviceModel);

            return this.Redirect("/");
        }
    }
}
