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

        public async Task<int> CreateAsync(CategoryServiceModel input)
        {
            var category = input.To<Category>();
            await this.categoryRepository.AddAsync(category);
            await this.categoryRepository.SaveChangesAsync();

            return category.Id;
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

        public IEnumerable<T> GetAllCategories<T>()
        {
            var categories = this.categoryRepository
                .All()
                .To<T>();

            return categories;
        }

        public IEnumerable<T> GetAllRootCategories<T>()
        {
            var categories = this.categoryRepository
                .All()
                .Where(c => c.ParentCategoryId == null)
                .To<T>();

            return categories;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var category = await this.categoryRepository.GetByIdAsync(id);

            return category.To<T>();
        }

        public async Task<T> GetByIdWithDeletedAsync<T>(int id)
        {
            var category = await this.categoryRepository.GetByIdWithDeletedAsync(id);

            return category.To<T>();
        }

        public async Task UpdateAsync(CategoryServiceModel input)
        {
            var category = await this.categoryRepository
                .GetByIdAsync(input.Id);
            category.Name = input.Name;
            category.ParentCategoryId = input.ParentCategoryId;
            category.Description = input.Description;
            category.ImageUrl = input.ImageUrl;

            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
