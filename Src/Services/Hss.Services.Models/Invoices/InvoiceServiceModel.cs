namespace Hss.Services.Models.Invoices
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class InvoiceServiceModel : IMapFrom<Invoice>, IMapTo<Invoice>
    {
        public string Id { get; set; }

        public int AddressId { get; set; }

        public string ClientId { get; set; }

        public InvoiceStatus Status { get; set; }

        public decimal Discount { get; set; }

        public decimal NetAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public bool UseDiscountPercentage { get; set; }

        public string OrderId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
