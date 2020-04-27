namespace Hss.Services.Data.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Orders;

    public interface IOrdersService
    {
        Task CreateAsync(OrderServiceModel input);

        int GetUnpaidRecurrentJobsCount(string id);

        int GetTotalMonthJobsTime(int serviceId, DateTime appointmentDate, string teamId = null);

        IEnumerable<T> GetAllWithUnpaidJobs<T>();

        Task<IEnumerable<T>> GetActiveOrdersByUserId<T>(string userId);
    }
}
