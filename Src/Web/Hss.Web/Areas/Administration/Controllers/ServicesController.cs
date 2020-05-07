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
            if (!this.categoriesService.CategoryExists(input.ParentCategoryId))
            {
                return this.NotFound();
            }

            var serviceModel = input.To<ServiceServiceModel>();
            var imageUrl = await this.cloudinaryService.UploadAsync(input.Image);
            serviceModel.ImageUrl = imageUrl;
            await this.servicesService.CreateAsync(serviceModel);

            return this.Redirect("/");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var serviceModel = await this.servicesService.GetByIdAsync<DeleteServiceViewModel>(id);
            if (serviceModel == null)
            {
                return this.NotFound();
            }

            return this.View(serviceModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteServiceViewModel input)
        {
            if (!this.servicesService.CheckIfServiceExists(input.Id))
            {
                return this.NotFound();
            }

            await this.servicesService.DeleteAsync(input.Id);

            return this.RedirectToAction("Details", "Categories", new { id = input.CategoryId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var serviceModel = await this.servicesService
                .GetByIdAsync<EditServiceInputModel>(id);

            if (serviceModel == null)
            {
                return this.NotFound();
            }

            return this.View(serviceModel);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Edit(EditServiceInputModel input)
        {
            if (
                !this.categoriesService.CategoryExists(input.CategoryId)
                || !this.servicesService.CheckIfServiceExists(input.Id))
            {
                return this.NotFound();
            }

            var serviceDto = input.To<ServiceServiceModel>();
            await this.servicesService.UpdateAsync(serviceDto);

            return this.RedirectToAction("Details", "Categories", new { id = input.CategoryId });
        }
    }
}
