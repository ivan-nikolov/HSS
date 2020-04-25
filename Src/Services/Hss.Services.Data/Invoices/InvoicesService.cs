namespace Hss.Services.Data.Invoices
{
    using System.Threading.Tasks;
    using Hss.Common;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Data.Orders;
    using Hss.Services.Data.Services;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoicesRepository;
        private readonly IOrdersService ordersService;
        private readonly IServicesService servicesService;

        public InvoicesService(
            IDeletableEntityRepository<Invoice> invoicesRepository,
            IOrdersService ordersService,
            IServicesService servicesService)
        {
            this.invoicesRepository = invoicesRepository;
            this.ordersService = ordersService;
            this.servicesService = servicesService;
        }

        public async Task CreateAsync(string orderId, string clientId, int serviceId, ServiceFrequency serviceFrequency, int addressId)
        {
            var servicePrice = this.servicesService.GetServicePrice(serviceId);
            var jobsCount = this.ordersService.GetUnpaindJobsCount(orderId);

            var discount = serviceFrequency != ServiceFrequency.Once ? GlobalConstants.DiscountForRecurrentService : 0;
            var netAmount = (jobsCount * servicePrice) * (1 - discount);

            var invoice = new Invoice()
            {
                AddressId = addressId,
                ClientId = clientId,
                Discount = discount,
                NetAmount = netAmount,
                TotalAmount = netAmount * (1 + GlobalConstants.VAT),
                OrderId = orderId,
                Status = InvoiceStatus.Pending,
                UseDiscountPercentage = true,
            };

            await this.invoicesRepository.AddAsync(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }
    }
}
