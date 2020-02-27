namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public Address()
        {
            this.Jobs = new HashSet<Job>();
            this.Invoices = new HashSet<Invoice>();

            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public string BuildingNumber { get; set; }

        public string Appartment { get; set; }

        public string StreetName { get; set; }

        public string Neighborhood { get; set; }

        public int ZipCode { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
