namespace Hss.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Services;

    public interface IServicesService
    {
        Task CreateAsync(ServiceServiceModel input);

        Task DeleteAsync(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task UpdateAsync(ServiceServiceModel input);

        Task<IEnumerable<T>> GetAllAsync<T>();

        IEnumerable<T> GetAllByCategoryId<T>(int categoryId);

        IEnumerable<T> GetAllByTeamCity<T>(int cityId);

        int GetCountByCategoryId(int categoryId);

        bool CheckIfServiceExists(int id);

        bool CheckIfServicesExist(IEnumerable<int> serviceIds);

        decimal GetServicePrice(int id);

        int GetServiceDuration(int id);
    }
}
