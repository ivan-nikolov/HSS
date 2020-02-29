namespace Hss.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UsersSeeder : ISeeder
    {
        private const string UsersPassword = "123456";

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (userManager.Users.Count() > 1)
            {
                return;
            }

            var admin = new ApplicationUser()
            {
                UserName = "Admin",
                FirstName = "Abram",
                LastName = "Davy",
                Email = "abram_davi@hss.com",
            };

            var client = new ApplicationUser()
            {
                UserName = "Client",
                FirstName = "Dayna",
                LastName = "Lennon",
                Email = "dayna_lennon@bmail.com",
            };

            var teamMember = new ApplicationUser()
            {
                UserName = "TeamMember",
                FirstName = "Santos",
                LastName = "Walton",
                Email = "santos_walton@bmail.com",
            };

            await SeedUser(userManager, admin, UsersPassword, GlobalConstants.AdministratorRoleName);
            await SeedUser(userManager, client, UsersPassword, GlobalConstants.ClientRoleName);
            await SeedUser(userManager, teamMember, UsersPassword, GlobalConstants.TeamMemberRoleName);
        }

        private static async Task SeedUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string roleName)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
