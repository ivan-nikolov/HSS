namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;
    using Hss.Data.Models.Enums;

    public class Contract : BaseDeletableModel<string>
    {
        public Contract()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;

            this.Orders = new HashSet<Order>();
        }

        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        public DateTime ActiveOn { get; set; }

        public DateTime BillingEndOn { get; set; }

        public BillingFrequency BillingFrequency { get; set; }

        public DateTime BillingStartOn { get; set; }

        public DateTime? CancelOn { get; set; }

        public DateTime Expiration { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
