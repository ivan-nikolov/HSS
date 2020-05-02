namespace Hss.Services.Data.Invoices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Data.Services;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Invoices;
    using Microsoft.EntityFrameworkCore;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoicesRepository;
        private readonly IServicesService servicesService;

        public InvoicesService(
            IDeletableEntityRepository<Invoice> invoicesRepository,
            IServicesService servicesService)
        {
            this.invoicesRepository = invoicesRepository;
            this.servicesService = servicesService;
        }

        public async Task CreateAsync(string orderId, string clientId, int serviceId, ServiceFrequency serviceFrequency, int addressId, int jobsCount)
        {
            var servicePrice = this.servicesService.GetServicePrice(serviceId);

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

        public async Task AddRange(IEnumerable<InvoiceServiceModel> invoices)
        {
            foreach (var invoice in invoices)
            {
                await this.invoicesRepository.AddAsync(invoice.To<Invoice>());
            }

            await this.invoicesRepository.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
            => await this.invoicesRepository.All()
            .Where(i => i.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

        public IQueryable<T> GetByClientId<T>(string clientId, DateTime beforeDate, DateTime afterDate, bool orderByCreatedDateDesc, InvoiceStatus status = InvoiceStatus.Pending)
        {
            var invoices = this.invoicesRepository.All()
                .Where(i => i.ClientId == clientId
                && i.Status == status
                && i.CreatedOn <= beforeDate
                && i.CreatedOn >= afterDate);

            if (orderByCreatedDateDesc)
            {
                invoices = invoices.OrderByDescending(i => i.CreatedOn);
            }
            else
            {
                invoices = invoices.OrderBy(i => i.CreatedOn);
            }

            return invoices.To<T>();
        }
    }
}
