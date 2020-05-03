namespace Hss.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Users;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UsersServiceTests : TestsBase
    {
        [Fact]
        public async Task AssignUserToTeamWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            var userId = Guid.NewGuid().ToString();
            var user = new ApplicationUser()
            {
                Id = userId,
            };

            dbContext.Add(user);
            await dbContext.SaveChangesAsync();

            var service = new UsersService(repository);
            var teamId = Guid.NewGuid().ToString();
            await service.AssignUserToTeam(userId, teamId);
            var result = dbContext.Users.FirstOrDefault();

            Assert.Equal(userId, result.Id);
            Assert.Equal(teamId, result.TeamId);
        }
    }
}
