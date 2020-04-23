namespace Hss.Services.Data.Teams
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Teams;
    using Microsoft.EntityFrameworkCore;

    public class TeamsService : ITeamsService
    {
        private readonly IDeletableEntityRepository<Team> teamsRepository;

        public TeamsService(IDeletableEntityRepository<Team> teamsRepository)
        {
            this.teamsRepository = teamsRepository;
        }

        public async Task<bool> CheckIfTeamExistsAsync(string id)
            => await this.teamsRepository.All()
            .Where(t => t.Id == id)
            .CountAsync() > 0;

        public async Task<string> CreateAsync(TeamServiceModel input)
        {
            var team = new Team()
            {
                CityId = input.CityId,
                Name = input.Name,
            };

            await this.teamsRepository.AddAsync(team);
            await this.teamsRepository.SaveChangesAsync();
            this.GenerateTeamServices(input, team);

            await this.teamsRepository.SaveChangesAsync();

            return team.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var team = await this.teamsRepository.GetByIdAsync(id);

            this.teamsRepository.Delete(team);
            await this.teamsRepository.SaveChangesAsync();
        }

        public T GetById<T>(string id)
            => this.teamsRepository.All()
            .Where(t => t.Id == id)
            .To<T>()
            .FirstOrDefault();

        public IEnumerable<T> GetTeamsInLocation<T>(int cityId)
            => this.teamsRepository.All()
            .Where(t => t.City.Id == cityId)
            .To<T>()
            .ToList();

        public async Task UpdateAsync(TeamServiceModel input)
        {
            var team = await this.teamsRepository.GetByIdAsync(input.Id);

            team.Name = input.Name;
            team.CityId = input.CityId;

            team.Services = new List<TeamService>();
            this.GenerateTeamServices(input, team);
            this.teamsRepository.Update(team);
            await this.teamsRepository.SaveChangesAsync();
        }

        private void GenerateTeamServices(TeamServiceModel input, Team team)
        {
            foreach (var serviceId in input.Services)
            {
                var teamService = new TeamService()
                {
                    ServiceId = serviceId,
                    TeamId = team.Id,
                };

                team.Services.Add(teamService);
            }
        }
    }
}
