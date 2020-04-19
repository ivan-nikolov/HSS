namespace Hss.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Count() > 1)
            {
                return;
            }

            var cleaning = new Category()
            {
                Name = "Cleaning",
                Description = "We are a reliable, professional, and ethical business that offers affordable end of tenancy cleaning, domestic cleaning, office cleaning, and commercial cleaning services to domestic and commercial customers.",
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1584526567/cgeovlsqzy5lynuygt6o.jpg",
            };

            var domesticCleaning = new Category()
            {
                Name = "Domestic Cleaning",
                Description = "We are passionate about cleaning and providing a reliable service.We care greatly about our customers, their homes and their families. We go above and beyond to ensure that our customer’s needs are always met!",
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1584379481/t1fyqtioygdrjlymparn.jpg",
                ParentCategory = cleaning,
            };

            var landscaping = new Category()
            {
                Name = "Landscaping",
                Description = "Investing in landscaping comes with lasting gains. This is especially true if you hire a landscaping professional for this job. With the services of landscaping experts, you can be sure of enjoying a natural environment. These experts have the knowledge and expertise required in ensuring you get your exterior look natural. More to this, a natural surrounding comes with an incredible package of enjoying good health.",
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1585907653/rnb3ncehkb9pyilmkbed.jpg",
            };

            var categories = new List<Category>()
            {
                cleaning,
                domesticCleaning,
                landscaping,
            };

            foreach (var category in categories)
            {
                dbContext.Add(category);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
