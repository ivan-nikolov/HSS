namespace Hss.Services.Data.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Services.Models.Orders;

    public interface IOrdersService
    {
        Task CancelAsync(string id);

        Task<string> CreateAsync(OrderServiceModel input);

        Task CompleteAsync(string id);

        bool CheckIfOrderExists(string id);

        T GetById<T>(string id);

        Task<T> GetByIdAsync<T>(string id);

        int GetUnpaidRecurrentJobsCount(string id);

        int GetTotalMonthJobsTime(int serviceId, DateTime appointmentDate, string teamId = null);

        IEnumerable<T> GetAllWithUnpaidJobs<T>();

        IQueryable<T> GetOrdersByUserId<T>(string userId);

        IQueryable<T> GetPendingOrders<T>();

        Task ConfirmAsync(string id, string teamId);
    }
}
