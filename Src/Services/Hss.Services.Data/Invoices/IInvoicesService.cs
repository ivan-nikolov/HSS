namespace Hss.Services.Data.Invoices
{
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;

    public interface IInvoicesService
    {
        Task CreateAsync(string orderId, string clientId, int serviceId, ServiceFrequency serviceFrequency, int addressId);
    }
}
