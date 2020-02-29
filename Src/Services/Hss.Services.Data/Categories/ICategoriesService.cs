namespace Hss.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Categories;

    public interface ICategoriesService
    {
        Task CreateAsync(CategoryServiceModel input);

        IEnumerable<CategoryServiceModel> GetAllRootCategories();
    }
}
