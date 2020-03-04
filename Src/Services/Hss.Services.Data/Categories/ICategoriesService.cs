namespace Hss.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Categories;

    public interface ICategoriesService
    {
        Task CreateAsync(CategoryServiceModel input);

        Task<CategoryServiceModel> GetByIdWithDeletedAsync(int id);

        Task<CategoryServiceModel> GetById(int id);

        IEnumerable<CategoryServiceModel> GetAllRootCategories();

        IEnumerable<CategoryServiceModel> GetAllCategories();

        Task DeleteAsync(int id);

        Task Update(CategoryServiceModel input);
    }
}
