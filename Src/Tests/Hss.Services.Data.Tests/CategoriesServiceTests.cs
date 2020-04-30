namespace Hss.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Categories;
    using Hss.Services.Models.Categories;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class CategoriesServiceTests : TestsBase
    {
        [Fact]
        public async Task CreateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);

            await service.CreateAsync(new CategoryServiceModel());
            var categoriesInDbCount = repository.All().ToList().Count();

            Assert.Equal(1, categoriesInDbCount);
        }

        [Fact]
        public async Task CategoryExistsReturnsTrueIfCategoryExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = service.CategoryExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CategoryExistsReturnsFalseIfCategoryDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = service.CategoryExists(2);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            var childCategoryOne = new Category()
            {
                Id = 2,
                ParentCategoryId = 1,
            };
            var childCategoryTwo = new Category()
            {
                Id = 3,
                ParentCategoryId = 1,
            };

            dbContext.Add(category);
            dbContext.Add(childCategoryOne);
            dbContext.Add(childCategoryTwo);
            await dbContext.SaveChangesAsync();
            await service.DeleteAsync(1);
            var categoriesInDbCount = dbContext.Categories.ToList().Count();

            Assert.Equal(0, categoriesInDbCount);
        }

        [Fact]
        public async Task GetAllCategoriesWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            var categoryOne = new Category()
            {
                Id = 2,
            };
            var categoryTwo = new Category()
            {
                Id = 3,
            };

            dbContext.Add(category);
            dbContext.Add(categoryOne);
            dbContext.Add(categoryTwo);
            await dbContext.SaveChangesAsync();
            var categories = service.GetAllCategories<CategoryServiceModel>();

            Assert.Equal(3, categories.Count());
        }

        [Fact]
        public async Task GetAllRootCategoriesWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            var categoryOne = new Category()
            {
                Id = 2,
            };
            var childCategoryOne = new Category()
            {
                Id = 3,
                ParentCategoryId = 1,
            };

            var childCategoryTwo = new Category()
            {
                Id = 4,
                ParentCategoryId = 2,
            };

            dbContext.Add(category);
            dbContext.Add(categoryOne);
            dbContext.Add(childCategoryOne);
            dbContext.Add(childCategoryTwo);
            await dbContext.SaveChangesAsync();
            var categories = service.GetAllRootCategories<CategoryServiceModel>();

            Assert.Equal(2, categories.Count());
        }

        [Fact]
        public async Task GetByIdAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await service.GetByIdAsync<CategoryServiceModel>(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsNullIfCategoryDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
            };

            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await service.GetByIdAsync<CategoryServiceModel>(2);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);

            var service = new CategoriesService(repository);
            var category = new Category()
            {
                Id = 1,
                Name = "1",
                ParentCategoryId = null,
                Description = "1",
                ImageUrl = "1",
            };

            dbContext.Add(category);
            await dbContext.SaveChangesAsync();

            var editedCategory = new CategoryServiceModel()
            {
                Id = 1,
                Name = "2",
                ParentCategoryId = 1,
                Description = "2",
                ImageUrl = "2",
            };

            await service.UpdateAsync(editedCategory);
            var result = dbContext.Categories.Find(1);

            Assert.Equal(editedCategory.Name, result.Name);
            Assert.Equal(editedCategory.ParentCategoryId, result.ParentCategoryId);
            Assert.Equal(editedCategory.Description, result.Description);
            Assert.Equal(editedCategory.ImageUrl, result.ImageUrl);
        }
    }
}
