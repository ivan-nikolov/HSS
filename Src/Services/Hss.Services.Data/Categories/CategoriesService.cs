namespace Hss.Services.Data.Categories
{
    using System;
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

            if (category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

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

        public IQueryable<CategoryServiceModel> GetAllCategories()
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

        public async Task<CategoryServiceModel> GetByIdAsync(int id)
        {
            var category = await this.categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

            return category.To<CategoryServiceModel>();
        }

        public async Task<CategoryServiceModel> GetByIdWithDeletedAsync(int id)
        {
            var category = await this.categoryRepository.GetByIdWithDeletedAsync(id);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

            return category.To<CategoryServiceModel>();
        }

        public async Task UpdateAsync(CategoryServiceModel input)
        {
            var category = await this.categoryRepository.GetByIdAsync(input.Id);
            if (category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

            category.Name = input.Name;
            category.ParentCategoryId = input.ParentCategoryId;

            this.categoryRepository.Update(category);
            await this.categoryRepository.SaveChangesAsync();
        }
    }
}
