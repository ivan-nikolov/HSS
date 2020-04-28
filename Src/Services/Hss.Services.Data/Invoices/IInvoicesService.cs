namespace Hss.Services.Data.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;
    using Hss.Services.Models.Invoices;

    public interface IInvoicesService
    {
        Task CreateAsync(string orderId, string clientId, int serviceId, ServiceFrequency serviceFrequency, int addressId, int jobsCount);

        Task AddRange(IEnumerable<InvoiceServiceModel> invoices);

        Task<T> GetByIdAsync<T>(string id);

        IQueryable<T> GetByClientId<T>(string clientId, DateTime beforeDate, DateTime afterDate, bool orderByCreatedDateDesc, InvoiceStatus status = InvoiceStatus.Pending);
    }
}
