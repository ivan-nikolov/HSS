namespace Hss.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(CategoryServiceModel input)
        {
            var category = input.To<Category>();
            await this.categoryRepository.AddAsync(category);
            await this.categoryRepository.SaveChangesAsync();
        }

        public IEnumerable<CategoryServiceModel> GetAllRootCategories()
        {
            var categories = this.categoryRepository
                .All()
                .Where(c => c.ParentCategoryId == null)
                .To<CategoryServiceModel>()
                .ToList();

            return categories;
        }
    }
}
