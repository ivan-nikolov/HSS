namespace Hss.Services.Data.JobsService
{
    using System;
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;

    public interface IJobsService
    {
        Task CreateAsync(string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmetnEndDate);

        Task CancelByOrderIdAsync(string orderId);
    }
}
