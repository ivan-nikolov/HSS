namespace Hss.Services.Data.JobsService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;

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

        private DateTime GetCurrentMothJobsDate(DateTime appointmentStartDate, DateTime appointment)
        {
            DateTime currentDate;
            var currentMonth = appointmentStartDate.AddMonths(appointment.Month - appointmentStartDate.Month);
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
                ServiceFrequency.Daily => todaysDate < appointmentStartDate ? appointmentStartDate : todaysDate.AddDays(1).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Weekly => todaysDate < appointmentStartDate ? appointmentStartDate : todaysDate.AddDays(7).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Monthly => this.GetCurrentMothJobsDate(appointmentStartDate, todaysDate),
                _ => default,
            };

            return startDate;
        }
    }
}
