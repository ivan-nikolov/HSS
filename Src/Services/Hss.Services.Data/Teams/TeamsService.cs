namespace Hss.Services.Data.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
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

        public bool HasFreeTeams(DateTime currentDate, ServiceFrequency serviceFrequency, int cityId)
        {
            var weekOfMonth = currentDate.GetWeekOfMonth();
            var dayOfWeek = (int)currentDate.DayOfWeek;

            var hasFreeTeams = false;

            if (serviceFrequency == ServiceFrequency.Once)
            {
                hasFreeTeams = this.teamsRepository.All()
                    .Where(t => t.CityId == cityId)
                    .Where(t => !t.Orders
                        .Select(o => o.Appointment)
                        .Any(a => currentDate >= a.StartDate && currentDate <= a.EndDate))
                    .Count() > 0;
            }
            else if (serviceFrequency == ServiceFrequency.Daily)
            {
                hasFreeTeams = this.teamsRepository.All()
                    .Where(t => t.CityId == cityId)
                    .Where(t => !t.Orders
                        .Select(o => o.Appointment)
                        .Any(a => currentDate.TimeOfDay >= a.StartDate.TimeOfDay
                        && currentDate.TimeOfDay <= a.EndDate.TimeOfDay))
                    .Count() > 0;
            }
            else if (serviceFrequency == ServiceFrequency.Weekly)
            {
                hasFreeTeams = this.teamsRepository.All()
                    .Where(t => t.CityId == cityId)
                    .Where(t => !t.Orders
                        .Select(o => o.Appointment)
                        .Any(a => a.DayOfWeek == dayOfWeek
                        && currentDate.TimeOfDay >= a.StartDate.TimeOfDay
                        && currentDate.TimeOfDay <= a.EndDate.TimeOfDay))
                    .Count() > 0;
            }
            else if (serviceFrequency == ServiceFrequency.Monthly)
            {
                hasFreeTeams = this.teamsRepository.All()
                    .Where(t => t.CityId == cityId)
                    .Where(t =>
                    !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Once)
                        .Select(o => o.Appointment)
                        .Any(a => a.StartDate == currentDate)
                    && !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Daily)
                        .Select(o => o.Appointment)
                        .Any(a => currentDate.TimeOfDay >= a.StartDate.TimeOfDay
                            && currentDate.TimeOfDay <= a.EndDate.TimeOfDay)
                    && !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Weekly)
                        .Select(o => o.Appointment)
                        .Any(a => a.DayOfWeek == dayOfWeek
                            && currentDate.TimeOfDay >= a.StartDate.TimeOfDay
                            && currentDate.TimeOfDay <= a.EndDate.TimeOfDay)
                    && !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Monthly)
                        .Select(o => o.Appointment)
                        .Any(a => a.WeekOfMonth == weekOfMonth
                            && a.DayOfWeek == dayOfWeek
                            && currentDate.TimeOfDay >= this.GetCurrentMothAppointmentDate(a, currentDate).TimeOfDay
                            && currentDate.TimeOfDay <= this.GetCurrentMothAppointmentDate(a, currentDate).AddHours(a.EndDate.Hour).TimeOfDay))
                .Count() > 0;
            }

            return hasFreeTeams;
        }

        private DateTime GetCurrentMothAppointmentDate(Appointment apt, DateTime appointment)
        {
            DateTime currentDate;
            var currentMonth = apt.StartDate.AddMonths(appointment.Month - apt.StartDate.Month);
            currentDate = currentMonth.AddDays((double)(apt.DayOfWeek - currentMonth.DayOfWeek));
            if (currentDate > currentMonth)
            {
                currentDate = currentDate.AddDays(-7);
            }

            return currentDate;
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
