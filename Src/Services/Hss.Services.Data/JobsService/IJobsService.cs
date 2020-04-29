namespace Hss.Services.Data.JobsService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;

    public interface IJobsService
    {
        Task CreateAsync(string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmetnEndDate);

        Task CancelByOrderIdAsync(string orderId);

        IQueryable<T> GetByTeamId<T>(string teamId);

        IQueryable<T> GetByUserId<T>(string userId);

        Task CompleteJobAsync(string id, string orderId, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmentEndDate);
    }
}
