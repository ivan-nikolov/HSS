namespace Hss.Services.Data.Invoices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;
    using Hss.Services.Models.Invoices;

    public interface IInvoicesService
    {
        Task CreateAsync(string orderId, string clientId, int serviceId, ServiceFrequency serviceFrequency, int addressId, int jobsCount);

        Task AddRange(IEnumerable<InvoiceServiceModel> invoices);
    }
}
