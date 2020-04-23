namespace Hss.Services.Data.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Countries;

    public interface ICountriesService
    {
        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<CountryServiceModel>> GetAll();

        IEnumerable<CountryServiceModel> GetAllHavingCities(bool hasServicesInCities = false);

        Task<T> GetByIdAsync<T>(int id);

        Task<bool> CheckIfCountryExists(int id);
    }
}
