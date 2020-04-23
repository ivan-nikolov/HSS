namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Data.CIties;
    using Hss.Services.Data.Services;
    using Hss.Services.Data.Teams;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Cities;
    using Hss.Services.Models.Services;
    using Hss.Services.Models.Teams;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Administration.Teams;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class TeamsController : AdministrationController
    {
        private readonly ITeamsService teamsService;
        private readonly ICitiesService citiesService;
        private readonly IServicesService servicesService;

        public TeamsController(ITeamsService teamsService, ICitiesService citiesService, IServicesService servicesService)
        {
            this.teamsService = teamsService;
            this.citiesService = citiesService;
            this.servicesService = servicesService;
        }

        public async Task<IActionResult> Create()
        {
            var cities = await this.citiesService.GetAllAsync<CitySelectModel>();
            var services = await this.servicesService.GetAllAsync<ServiceSelectModel>();

            var model = new CreateTeamInputModel()
            {
                Cities = cities
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }).ToList(),
                AllServices = services
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }).ToList(),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Create(CreateTeamInputModel input)
        {
            if (!await this.citiesService.CheckIfCityExistsAsync(input.CityId))
            {
                return this.NotFound();
            }

            if (!this.servicesService.CheckIfServicesExist(input.Services))
            {
                return this.NotFound();
            }

            var serviceModel = input.To<TeamServiceModel>();
            var id = await this.teamsService.CreateAsync(serviceModel);

            return this.RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = this.teamsService.GetById<EditTeamInputModel>(id);
            if (model == null)
            {
                return this.NotFound();
            }

            var cities = await this.citiesService.GetAllAsync<CitySelectModel>();
            var services = await this.servicesService.GetAllAsync<ServiceSelectModel>();
            model.AllServices = services
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }).ToList();
            model.Cities = cities
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }).ToList();

            return this.View(model);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> Edit(EditTeamInputModel input)
        {
            if (!await this.teamsService.CheckIfTeamExistsAsync(input.Id))
            {
                return this.NotFound();
            }

            if (!await this.citiesService.CheckIfCityExistsAsync(input.CityId))
            {
                return this.NotFound();
            }

            if (!this.servicesService.CheckIfServicesExist(input.Services))
            {
                return this.NotFound();
            }

            var serviceModel = input.To<TeamServiceModel>();
            await this.teamsService.UpdateAsync(serviceModel);

            return this.RedirectToAction("Details", new { serviceModel.Id });
        }

        public IActionResult Delete(string id)
        {
            var viewModel = this.teamsService.GetById<DeleteTeamViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            // TODO: Implement Team Delete view
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteTeamViewModel input)
        {
            if (!await this.teamsService.CheckIfTeamExistsAsync(input.Id))
            {
                return this.NotFound();
            }

            await this.teamsService.DeleteAsync(input.Id);

            return this.Redirect("/Administration/Teams");
        }

        public IActionResult Details(string id)
        {
            var team = this.teamsService.GetById<DetailsViewModel>(id);
            if (team == null)
            {
                return this.NotFound();
            }

            // TODO Implement Team Details View
            return this.View(team);
        }
    }
}
