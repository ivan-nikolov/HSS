namespace Hss.Web.ViewModels.Invoices
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class InvoiceViewModel : IMapFrom<Invoice>
    {
        public string Id { get; set; }

        public InvoiceStatus Status { get; set; }

        public string OrderServiceName { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal TotalAmount { get; set; }

        public string OrderId { get; set; }
    }
}
