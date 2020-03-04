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

        public async Task DeleteAsync(int id)
        {
            var category = await this.categoryRepository.GetByIdAsync(id);

            var childCategories = this.categoryRepository.All()
                .Where(c => c.ParentCategoryId == id)
                .Select(c => c.Id)
                .ToList();

            foreach (var childCategoryId in childCategories)
            {
                await this.DeleteAsync(childCategoryId);
            }

            this.categoryRepository.Delete(category);
            await this.categoryRepository.SaveChangesAsync();
        }

        public IEnumerable<CategoryServiceModel> GetAllCategories()
        {
            var categories = this.categoryRepository
                .All()
                .To<CategoryServiceModel>();

            return categories;
        }

        public IEnumerable<CategoryServiceModel> GetAllRootCategories()
        {
            var categories = this.categoryRepository
                .All()
                .Where(c => c.ParentCategoryId == null)
                .To<CategoryServiceModel>();

            return categories;
        }

        public async Task<CategoryServiceModel> GetById(int id)
        {
            var category = await this.categoryRepository.GetByIdAsync(id);

            return category.To<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> GetByIdWithDeletedAsync(int id)
        {
            var category = await this.categoryRepository.GetByIdWithDeletedAsync(id);
            return category.To<CategoryServiceModel>();
        }

        public async Task Update(CategoryServiceModel input)
        {
            var category = this.GetById(input.Id).To<Category>();
            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
