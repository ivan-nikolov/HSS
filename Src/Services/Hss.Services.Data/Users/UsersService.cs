namespace Hss.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task AssignUserToTeam(string userId, string teamId)
        {
            var user = this.usersRepository.All()
                .Where(u => u.Id == userId)
                .FirstOrDefault();

            user.TeamId = teamId;
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public IQueryable<ApplicationUser> GetAllUsers()
            => this.usersRepository
            .All();

        public async Task<string> GetTeamId(string id)
            => (await this.usersRepository
            .All()
            .FirstOrDefaultAsync(u => u.Id == id))
            .TeamId;
    }
}
