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
            this.Jobs = new HashSet<Job>();
            this.Invoices = new HashSet<Invoice>();
        }

        public BillingFrequency BillingFrequency { get; set; }

        public OrderStatus Status { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }

        public string AppointmetnId { get; set; }

        public Appointment Appointment { get; set; }

        public int AddresId { get; set; }

        public Address Address { get; set; }

        public string TeamId { get; set; }

        public Team Team { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
