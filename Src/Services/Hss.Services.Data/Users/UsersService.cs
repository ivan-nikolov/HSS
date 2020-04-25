namespace Hss.Services.Data.Users
{
    using System.Linq;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public IQueryable<ApplicationUser> GetAllUsers()
            => this.usersRepository
            .All();
    }
}
