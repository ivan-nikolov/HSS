namespace Hss.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Models;

    public class ServicesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Services.Count() > 1)
            {
                return;
            }

            var endOfTEnancyCleaning = new Service()
            {
                Name = "End of Tenancy Cleaning",
                Description = "Cleaning Express can help you prepare your property for new tenants and make it look welcoming and nice for them. We also offer handyman services if the premises need a bit of repair before new people move in. Contact us to find out how we can assist you.",
                CategoryId = dbContext.Categories.FirstOrDefault(x => x.Name == "Domestic Cleaning").Id,
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1587303419/living-room-couch-interior-room-584399_fwyzny.jpg",
                IsRecurrent = false,
                Price = 100.00M,
                DurationInHours = 4,
            };

            var housekeeping = new Service()
            {
                Name = "Housekeeping",
                Description = "We go beyond basic cleaning duties at Cleaning Express and are able to offer housekeeping services.Most of our staff will be able to fulfil a variety of duties including laundry, changing beds, organising clothes, shopping, basic help for the elderly and much more.Our housekeeping services are perfect for anyone who needs a little bit of extra help around the home.We have set up our housekeeping team to be there for the people that don’t need a cleaner but do need someone to come in and help with a few errands. We know how busy life can be and our team is there for you! You take care of the important stuff, and we’ll take care of the chores.",
                CategoryId = dbContext.Categories.FirstOrDefault(x => x.Name == "Domestic Cleaning").Id,
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1587304066/washing-machine-2668472_640_goxte6.jpg",
                IsRecurrent = true,
                Price = 80,
                DurationInHours = 2,
            };

            var fanceInstallation = new Service()
            {
                Name = "Fance Installation",
                Description = "As part of your fence installation, we provide start-to-finish project management. Our local service provider will pull any necessary permits, professionally install your fence, and clean up and dispose of any job-related debris. (Disposal fees may apply.) Before leaving your property, the installer will walk you through final inspection of the fence and answer any questions you may have.",
                CategoryId = dbContext.Categories.FirstOrDefault(x => x.Name == "Landscaping").Id,
                ImageUrl = "https://res.cloudinary.com/home-services-and-solutions/image/upload/v1585915074/qmekebowxixwkmrjxspl.jpg",
                IsRecurrent = false,
                Price = 150,
                DurationInHours = 20,
            };

            var services = new List<Service>()
            {
                endOfTEnancyCleaning,
                housekeeping,
                fanceInstallation,
            };

            foreach (var service in services)
            {
                dbContext.Services.Add(service);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
