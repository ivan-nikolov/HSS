namespace Hss.Services.Data.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
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

        public bool HasFreeTeams(DateTime startDate, DateTime endDate, int cityId, int serviceId, ServiceFrequency serviceFrequency)
        {
            var freeTeams = this.GetFreeTeams<TeamServiceModel>(startDate, endDate, cityId, serviceId, serviceFrequency).ToList();

            return freeTeams.Count > 0;
        }

        public IEnumerable<T> GetFreeTeams<T>(DateTime startDate, DateTime endDate, int cityId, int serviceId, ServiceFrequency serviceFrequency)
        {
            var weekOfMonth = startDate.GetWeekOfMonth();
            var dayOfWeek = (int)startDate.DayOfWeek;

            Expression<Func<Team, bool>> checkSingleOrders = t =>
                    !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Once
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => (startDate >= a.StartDate && startDate <= a.EndDate)
                            || (startDate < a.StartDate && endDate >= a.StartDate));

            Expression<Func<Team, bool>> checkDailyOrders = t => !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Daily
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => (serviceFrequency == ServiceFrequency.Once ? a.StartDate < startDate : true)
                            && ((startDate.Hour >= a.StartDate.Hour && startDate.Hour <= a.EndDate.Hour)
                            || (startDate.Hour < a.StartDate.Hour && endDate.Hour >= a.StartDate.Hour)));

            Expression<Func<Team, bool>> checkWeeklyOrders = t => !t.Orders
                        .Where(o => o.ServiceFrequency == ServiceFrequency.Weekly
                            && (o.Status == OrderStatus.InProgress || o.Status == OrderStatus.Pending))
                        .Select(o => o.Appointment)
                        .Any(a => (serviceFrequency == ServiceFrequency.Once ? a.StartDate < startDate : true)
                            && (a.DayOfWeek == dayOfWeek
                            && ((startDate.Hour >= a.StartDate.Hour && startDate.Hour <= a.EndDate.Hour)
                            || (startDate.Hour < a.StartDate.Hour && endDate.Hour >= a.StartDate.Hour))));

            var teams = this.teamsRepository.All()
                .Where(t =>
                t.CityId == cityId
                && t.Services.Select(s => s.ServiceId).Contains(serviceId)
                && t.TeamMembers.Any());

            teams = teams.Where(checkSingleOrders);
            teams = teams.Where(checkDailyOrders);
            teams = teams.Where(checkWeeklyOrders);

            var freeTeams = this.CheckMonthlyFreeTeams<T>(startDate, endDate, teams, serviceFrequency);

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

        private bool CheckMonthlyAppointments(Appointment appointment, DateTime startDate, DateTime endDate, ServiceFrequency serviceFrequency)
        {
            var currentMonthAppointmentStartDate = this.GetCurrentMothAppointmentDate(appointment, startDate);
            var currentMontAppointmentEndDate = currentMonthAppointmentStartDate.Date.AddHours(appointment.EndDate.Hour);

            var result = serviceFrequency switch
            {
                ServiceFrequency.Daily => (startDate.TimeOfDay >= currentMonthAppointmentStartDate.TimeOfDay && startDate.TimeOfDay <= currentMontAppointmentEndDate.TimeOfDay)
                            || (startDate.TimeOfDay < currentMonthAppointmentStartDate.TimeOfDay && endDate.TimeOfDay >= currentMonthAppointmentStartDate.TimeOfDay),
                ServiceFrequency.Weekly => appointment.DayOfWeek == (int)currentMonthAppointmentStartDate.DayOfWeek
                            && ((startDate.TimeOfDay >= currentMonthAppointmentStartDate.TimeOfDay && startDate.TimeOfDay <= currentMontAppointmentEndDate.TimeOfDay)
                            || (startDate.TimeOfDay < currentMonthAppointmentStartDate.TimeOfDay && endDate.TimeOfDay >= currentMonthAppointmentStartDate.TimeOfDay)),
                _ => (startDate >= currentMonthAppointmentStartDate && startDate <= currentMontAppointmentEndDate)
                    || (startDate < currentMonthAppointmentStartDate && endDate >= currentMonthAppointmentStartDate),
            };

            return result;
        }

        private IEnumerable<T> CheckMonthlyFreeTeams<T>(DateTime currentDate, DateTime endDate, IQueryable<Team> teams, ServiceFrequency serviceFrequency)
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
                        .Any(a => (serviceFrequency == ServiceFrequency.Once ? a.StartDate < currentDate : true)
                            && this.CheckMonthlyAppointments(a, currentDate, endDate, serviceFrequency)))
                .Select(t => t.To<T>())
                .ToList();

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
