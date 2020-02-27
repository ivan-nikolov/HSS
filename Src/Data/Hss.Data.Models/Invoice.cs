namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;
    using Hss.Data.Models.Enums;

    public class Invoice : BaseDeletableModel<string>
    {
        public Invoice()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public int AddressId { get; set; }

        public Address Address { get; set; }

        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        public InvoiceStatus Status { get; set; }

        public decimal Discount { get; set; }

        public decimal NetAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public bool UseDiscountPercentage { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
