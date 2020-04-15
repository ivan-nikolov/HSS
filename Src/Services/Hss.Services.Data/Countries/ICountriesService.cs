namespace Hss.Services.Data.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountriesService
    {
        Task CreateAsync(string name);

        Task DeleteAsync(int id);

        Task<IEnumerable<T>> GetAll<T>();

        Task<T> GetByIdAsync<T>(int id);

        Task<bool> CheckIfCountryExists(int id);
    }
}
