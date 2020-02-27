namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;
    using Hss.Data.Models.Enums;

    public class Order : BaseDeletableModel<string>
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public int ServiceFrequencyInDays { get; set; }

        public OrderStatus Status { get; set; }

        public string InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        public string ContractId { get; set; }

        public Contract Contract { get; set; }

        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
