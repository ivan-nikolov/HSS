namespace Hss.Services.Data.CIties
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Cities;

    public interface ICitiesService
    {
        Task CreateAsync(string name, int countryId);

        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetByCountryIdAsync<T>(int countryId);

        Task<IEnumerable<CityServiceModel>> GetByCountryIdAsync(int countryId);

        Task DeleteByCountryIdAsync(int countryId);

        Task<bool> CheckIfCityExists(int id);

        Task<T> GetById<T>(int id);
    }
}
