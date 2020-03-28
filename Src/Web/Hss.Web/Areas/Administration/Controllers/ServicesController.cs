namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Hss.Services;
    using Hss.Services.Data.Categories;
    using Hss.Services.Data.Services;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Administration.Services;
    using Microsoft.AspNetCore.Mvc;

    public class ServicesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IServicesService servicesService;
        private readonly ICloudinaryService cloudinaryService;

        public ServicesController(
            ICategoriesService categoriesService,
            IServicesService servicesService,
            ICloudinaryService cloudinaryService)
        {
            this.categoriesService = categoriesService;
            this.servicesService = servicesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Create(CreateServiceInputModel input)
        {
            var category = await this.categoriesService
                .GetByIdAsync<CreateServiceInputModel>(input.CategoryId);
            if (category == null)
            {
                return this.NotFound();
            }

            var serviceModel = input.To<ServiceServiceModel>();
            var imageUrl = await this.cloudinaryService.UploadAsync(input.Image);
            serviceModel.ImageUrl = imageUrl;
            await this.servicesService.CreateAsync(serviceModel);

            return this.Redirect("/");
        }
    }
}
