namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Data.CIties;
    using Hss.Services.Data.Countries;
    using Hss.Services.Models.Countries;
    using Hss.Web.Filters;
    using Hss.Web.ViewModels.Administration.OperationalLocations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class OperationalLocationsController : AdministrationController
    {
        private readonly ICountriesService countriesService;
        private readonly ICitiesService citiesService;

        public OperationalLocationsController(ICountriesService countriesService, ICitiesService citiesService)
        {
            this.countriesService = countriesService;
            this.citiesService = citiesService;
        }

        public async Task<IActionResult> Index()
        {
            var locations = new LocationsListViewModel()
            {
                Locations = await this.countriesService.GetAll<LocationsIndexViewModel>(),
            };

            return this.View(locations);
        }

        public IActionResult CreateCountry()
        {
            return this.View();
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> CreateCountry(CreateCountryInputModel input)
        {
            await this.countriesService.CreateAsync(input.Name);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await this.countriesService.GetByIdAsync<DeleteCountryInputModel>(id);
            if (country == null)
            {
                return this.NotFound();
            }

            return this.View(country);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCountry(DeleteCountryInputModel input)
        {
            var country = this.countriesService.GetByIdAsync<DeleteCountryInputModel>(input.Id);
            if (country == null)
            {
                return this.NotFound();
            }

            await this.countriesService.DeleteAsync(country.Id);

            return this.RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateCity()
        {
            var countries = await this.countriesService.GetAll<CountryServiceModel>();
            var model = new CreateCityInputModel()
            {
                Countries = countries
                .Select(c => new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                }),
            };

            return this.View(model);
        }

        [HttpPost]
        [ModelValidationActionFilter]
        public async Task<IActionResult> CreateCity(CreateCityInputModel input)
        {
            if (!await this.countriesService.CheckIfCountryExists(input.CountryId))
            {
                return this.NotFound();
            }

            await this.citiesService.CreateAsync(input.Name, input.CountryId);
            return this.RedirectToAction("Index");
        }

        public IActionResult DeleteCity(int id)
        {
            var city = this.citiesService.GetById<DeleteCityInputModel>(id);
            if (city == null)
            {
                return this.NotFound();
            }

            return this.View(city);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCity(DeleteCityInputModel input)
        {
            if (!await this.citiesService.CheckIfCityExists(input.Id))
            {
                return this.NotFound();
            }

            await this.citiesService.DeleteAsync(input.Id);
            return this.RedirectToAction("Index");
        }
    }
}
