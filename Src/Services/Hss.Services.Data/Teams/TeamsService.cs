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

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.teamsRepository.All()
            .To<T>()
            .ToListAsync();

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

        public bool HasFreeTeams(DateTime startDate, DateTime endDate, int cityId, int serviceId)
        {
            var freeTeams = this.GetFreeTeams<TeamServiceModel>(startDate, endDate, cityId, serviceId).ToList();

            return freeTeams.Count > 0;
        }

        public IEnumerable<T> GetFreeTeams<T>(DateTime startDate, DateTime endDate, int cityId, int serviceId)
        {
            var weekOfMonth = startDate.GetWeekOfMonth();
            var dayOfWeek = (int)startDate.DayOfWeek;
            var teams = this.teamsRepository.All()
                .Where(t =>
                t.CityId == cityId
                && t.Services.Select(s => s.ServiceId).Contains(serviceId)
                && t.TeamMembers.Any());

            var freeTeams = teams
                    .Where(t =>
                    !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Once
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => (startDate >= a.StartDate && startDate <= a.EndDate)
                            || (startDate < a.StartDate && endDate >= a.StartDate))
                    && !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Daily
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => (startDate.TimeOfDay >= a.StartDate.TimeOfDay && startDate.TimeOfDay <= a.EndDate.TimeOfDay)
                            || (startDate.TimeOfDay < a.StartDate.TimeOfDay && endDate.TimeOfDay >= a.StartDate.TimeOfDay))
                    && !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Weekly
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => a.DayOfWeek == dayOfWeek
                            && ((startDate.TimeOfDay >= a.StartDate.TimeOfDay && startDate.TimeOfDay <= a.EndDate.TimeOfDay)
                            || (startDate.TimeOfDay < a.StartDate.TimeOfDay && endDate.TimeOfDay >= a.StartDate.TimeOfDay)))
                    && this.CheckMonthlyFreeTeams(startDate, endDate, teams))
                    .To<T>()
                    .ToList();

            return freeTeams;
        }

        private DateTime GetCurrentMothAppointmentDate(Appointment apt, DateTime appointment)
        {
            DateTime currentDate;
            var currentMonth = apt.StartDate.AddMonths(appointment.Month - apt.StartDate.Month);
            currentDate = currentMonth.AddDays((double)(apt.DayOfWeek - currentMonth.DayOfWeek));
            if (currentDate.Month > currentMonth.Month)
            {
                currentDate = currentDate.AddDays(-7);
            }

            return currentDate;
        }

        private bool CheckMonthlyAppointments(Appointment appointment, DateTime startDate, DateTime endDate)
        {
            var currentMonthAppointmentStartDate = this.GetCurrentMothAppointmentDate(appointment, startDate);
            var currentMontAppointmentEndDate = currentMonthAppointmentStartDate.Date.AddHours(appointment.EndDate.Hour);
            var result = (startDate >= currentMonthAppointmentStartDate && startDate <= currentMontAppointmentEndDate)
                    || (startDate < currentMonthAppointmentStartDate && endDate >= currentMonthAppointmentStartDate);

            return result;
        }

        private bool CheckMonthlyFreeTeams(DateTime currentDate, DateTime endDate, IQueryable<Team> teams)
        {
            var allTeams = teams
                   .Select(t => new Team()
                   {
                       Id = t.Id,
                       Orders = t.Orders
                       .Select(o => new Order()
                       {
                           ServiceFrequency = o.ServiceFrequency,
                           Appointment = o.Appointment,
                           Status = o.Status,
                       }).ToList(),
                   }).ToList();

            var result = allTeams
                .Where(t => !t.Orders
                    .Where(o => o.ServiceFrequency == ServiceFrequency.Monthly
                        && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                    .Select(o => o.Appointment)
                    .All(a => this.CheckMonthlyAppointments(a, currentDate, endDate)))
                .Select(t => t.Id)
                .ToList().Count > 0;

            return result;
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
