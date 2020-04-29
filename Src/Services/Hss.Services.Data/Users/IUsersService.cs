namespace Hss.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;
    using Hss.Data.Models;

    public interface IUsersService
    {
        IQueryable<ApplicationUser> GetAllUsers();

        Task<string> GetTeamId(string id);
    }
}
