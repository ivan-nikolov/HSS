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

        Task<IEnumerable<CityServiceModel>> GetByCountryIdAsync(int countryId, bool hasServices = false);

        Task DeleteByCountryIdAsync(int countryId);

        Task<bool> CheckIfCityExistsAsync(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task<IEnumerable<T>> GetAllAsync<T>();
    }
}
