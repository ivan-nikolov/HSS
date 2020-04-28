namespace Hss.Services.Cron
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Models.Enums;
    using Hss.Services.Data.Invoices;
    using Hss.Services.Data.Orders;
    using Hss.Services.Models.Invoices;
    using Hss.Services.Models.Orders;

    public class RecurrentOrdersInvoiceGeneratorJob
    {
        public const string CronSchedule = "0 4 1 * *";

        private readonly IOrdersService ordersService;
        private readonly IInvoicesService invoicesService;

        public RecurrentOrdersInvoiceGeneratorJob(IOrdersService ordersService,
            IInvoicesService invoicesService)
        {
            this.ordersService = ordersService;
            this.invoicesService = invoicesService;
        }

        public async Task GenerateInvoices()
        {
            var orders = this.ordersService.GetAllWithUnpaidJobs<OrderServiceModel>();
            var invoices = new List<InvoiceServiceModel>();
            foreach (var order in orders)
            {
                invoices.Add(this.GenerateInvoice(order));
            }

            await this.invoicesService.AddRange(invoices);
        }

        private InvoiceServiceModel GenerateInvoice(OrderServiceModel order)
        {
            var servicePrice = order.Service.Price;

            var discount = order.ServiceFrequency != ServiceFrequency.Once ? GlobalConstants.DiscountForRecurrentService : 0;
            var netAmount = (order.Jobs.Count * servicePrice) * (1 - discount);

            var invoice = new InvoiceServiceModel()
            {
                AddressId = order.AddressId,
                ClientId = order.ClientId,
                Discount = discount,
                NetAmount = netAmount,
                TotalAmount = netAmount * (1 + GlobalConstants.VAT),
                OrderId = order.Id,
                Status = InvoiceStatus.Pending,
                UseDiscountPercentage = true,
            };

            return invoice;
        }
    }
}
