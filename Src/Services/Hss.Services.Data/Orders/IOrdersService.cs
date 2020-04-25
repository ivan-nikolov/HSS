namespace Hss.Services.Data.Orders
{
    using System;
    using System.Threading.Tasks;

    using Hss.Services.Models.Orders;

    public interface IOrdersService
    {
        Task CreateAsync(OrderServiceModel input);

        int GetUnpaidRecurrentJobsCount(string id);

        int GetTotalMontJobsTime(int serviceId, DateTime appointmentDate, string teamId = null);
    }
}
