namespace Hss.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class RootAdminSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any())
            {
                return;
            }

            var username = configuration["RootAdmin:Username"];
            var firstName = configuration["RootAdmin:FirstName"];
            var lastName = configuration["RootAdmin:LastName"];
            var password = configuration["RootAdmin:Password"];
            var email = configuration["RootAdmin:Email"];

            var rootAdmin = new ApplicationUser()
            {
                UserName = username,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            };

            var result = await userManager.CreateAsync(rootAdmin, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(rootAdmin, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
