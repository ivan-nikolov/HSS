namespace Hss.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Models;
    using Hss.Services.Data.Teams;
    using Hss.Services.Data.Users;
    using Hss.Services.Mapping;
    using Hss.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly ITeamsService teamsService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            ITeamsService teamsService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.teamsService = teamsService;
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

        public async Task<IActionResult> AssignUserToTeam(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.BadRequest();
            }

            var roles = await this.userManager.GetRolesAsync(user);
            if (roles.Contains(GlobalConstants.AdministratorRoleName) || roles.Contains(GlobalConstants.TeamMemberRoleName))
            {
                return this.BadRequest();
            }

            var teams = (await this.teamsService.GetAll<AssignUserToTeamTeamViewModel>()).ToList();
            var model = new AssignUserToTeamInputModel()
            {
                Username = user.UserName,
                UserId = user.Id,
                Teams = teams
                .Select(t => new SelectListItem()
                {
                    Value = t.Id,
                    Text = t.Name,
                }),
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserToTeam(AssignUserToTeamInputModel input)
        {
            var user = await this.userManager.FindByIdAsync(input.UserId);
            if (user == null)
            {
                return this.BadRequest();
            }

            if (!await this.teamsService.CheckIfTeamExistsAsync(input.TeamId))
            {
                return this.BadRequest();
            }

            var roles = await this.userManager.GetRolesAsync(user);
            if (roles.Contains(GlobalConstants.AdministratorRoleName) || roles.Contains(GlobalConstants.TeamMemberRoleName))
            {
                return this.BadRequest();
            }

            await this.userManager.AddToRoleAsync(user, GlobalConstants.TeamMemberRoleName);
            await this.userManager.RemoveFromRoleAsync(user, GlobalConstants.ClientRoleName);

            await this.usersService.AssignUserToTeam(input.UserId, input.TeamId);

            return this.View();
        }
    }
}
