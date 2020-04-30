namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(string searchString)
        {
            var users = this.userManager.Users;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString));
            }

            var resultUsers = await users.ToListAsync();
            var modelUsers = resultUsers
                .Select(u => u.To<UserAllViewModel>())
                .ToList();

            foreach (var user in modelUsers)
            {
                user.UserRoles = await this.userManager
                .GetRolesAsync(resultUsers.FirstOrDefault(x => x.Id == user.Id));
            }

            var model = new UserAllListModel()
            {
                Users = modelUsers,
            };

            return this.View(model);
        }
    }
}
