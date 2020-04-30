namespace Hss.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Categories;

    public interface ICategoriesService
    {
        Task<int> CreateAsync(CategoryServiceModel input);

        Task<T> GetByIdAsync<T>(int id);

        IEnumerable<T> GetAllRootCategories<T>();

        IEnumerable<T> GetAllCategories<T>();

        Task DeleteAsync(int id);

        Task UpdateAsync(CategoryServiceModel input);

        bool CategoryExists(int id);
    }
}
