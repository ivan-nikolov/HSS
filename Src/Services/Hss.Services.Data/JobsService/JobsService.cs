namespace Hss.Services.Data.JobsService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class JobsService : IJobsService
    {
        private readonly IDeletableEntityRepository<Job> jobsRepository;

        public JobsService(IDeletableEntityRepository<Job> jobsRepository)
        {
            this.jobsRepository = jobsRepository;
        }

        public async Task CancelByOrderIdAsync(string orderId)
        {
            var jobs = this.jobsRepository.All()
                .Where(j => j.OrderId == orderId && j.JobStatus == JobStatus.InProgress)
                .ToList();

            foreach (var job in jobs)
            {
                job.JobStatus = JobStatus.Cancelled;
                this.jobsRepository.Update(job);
            }

            await this.jobsRepository.SaveChangesAsync();
        }

        public async Task CompleteJobAsync(string id, string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmentEndDate)
        {
            var job = this.jobsRepository.All()
                .FirstOrDefault(j => j.Id == id);

            job.JobStatus = JobStatus.Done;
            this.jobsRepository.Update(job);
            await this.jobsRepository.SaveChangesAsync();

            if (serviceFrequency != ServiceFrequency.Once)
            {
                await this.CreateAsync(orderId, serviceFrequency, appointmentStartDate, appointmentEndDate);
            }
        }

        public async Task CreateAsync(string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmetnEndDate)
        {
            var startDate = this.GetNextJobStartDate(serviceFrequency, appointmentStartDate);

            var job = new Job()
            {
                OrderId = orderId,
                StartDate = startDate,
                FinishDate = startDate.Date.AddHours(appointmetnEndDate.TimeOfDay.Hours),
                JobStatus = JobStatus.InProgress,
            };

            await this.jobsRepository.AddAsync(job);
            await this.jobsRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetByTeamId<T>(string teamId)
            => this.jobsRepository.All()
            .Where(j => j.Order.TeamId == teamId)
            .OrderBy(j => j.StartDate)
            .To<T>();

        public IQueryable<T> GetByUserId<T>(string userId)
            => this.jobsRepository.All()
            .Where(j => j.Order.ClientId == userId)
            .OrderByDescending(j => j.StartDate)
            .To<T>();

        private DateTime GetCurrentMothJobsDate(DateTime appointmentStartDate, DateTime appointment)
        {
            DateTime currentDate;

            var currentMonth = appointment < appointmentStartDate
                ? appointmentStartDate
                : appointmentStartDate.AddMonths(appointment.Month - appointmentStartDate.Month);

            currentDate = currentMonth.AddDays((double)(appointmentStartDate.DayOfWeek - currentMonth.DayOfWeek));
            if (currentDate > currentMonth)
            {
                currentDate = currentDate.AddDays(-7);
            }

            return currentDate;
        }

        private DateTime GetNextJobStartDate(ServiceFrequency serviceFrequency, DateTime appointmentStartDate)
        {
            var todaysDate = DateTime.UtcNow.Date;

            var startDate = serviceFrequency switch
            {
                ServiceFrequency.Once => appointmentStartDate,
                ServiceFrequency.Daily => todaysDate < appointmentStartDate.Date
                    ? appointmentStartDate
                    : todaysDate.AddDays(1).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Weekly => todaysDate.Date < appointmentStartDate.Date
                    ? appointmentStartDate
                    : todaysDate.AddDays(7).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Monthly => this.GetCurrentMothJobsDate(appointmentStartDate, todaysDate),
                _ => default,
            };

            return startDate;
        }
    }
}
