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

        IEnumerable<T> GetAllByCategoryId<T>(int categoryId);

        int GetCountByCategoryId(int categoryId);

        bool ServiceExists(int id);
    }
}
