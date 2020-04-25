namespace Hss.Services.Data.JobsService
{
    using System;
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

        public async Task CreateAsync(string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmetnEndDate)
        {
            var job = new Job()
            {
                OrderId = orderId,
                StartDate = this.GetNextJobStartDate(serviceFrequency, appointmentStartDate),
                FinishDate = appointmentStartDate.AddHours(appointmetnEndDate.TimeOfDay.Hours),
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
                ServiceFrequency.Daily => todaysDate.AddDays(1).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Weekly => todaysDate.AddDays(7).AddHours(appointmentStartDate.TimeOfDay.Hours),
                ServiceFrequency.Monthly => this.GetCurrentMothJobsDate(appointmentStartDate, todaysDate),
                _ => default,
            };

            return startDate;
        }
    }
}
