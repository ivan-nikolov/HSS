namespace Hss.Services.Data.Users
{
    using System.Linq;

    using Hss.Data.Models;

    public interface IUsersService
    {
        IQueryable<ApplicationUser> GetAllUsers();
    }
}
