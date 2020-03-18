namespace Hss.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Models.Categories;

    public interface ICategoriesService
    {
        Task CreateAsync(CategoryServiceModel input);

        Task<CategoryServiceModel> GetByIdWithDeletedAsync(int id);

        Task<CategoryServiceModel> GetByIdAsync(int id);

        IEnumerable<CategoryServiceModel> GetAllRootCategories();

        IQueryable<CategoryServiceModel> GetAllCategories();

        Task DeleteAsync(int id);

        Task UpdateAsync(CategoryServiceModel input);
    }
}
