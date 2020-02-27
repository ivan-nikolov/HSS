namespace Hss.Data.Models
{
    using System;

    using Hss.Data.Common.Models;

    public class Job : BaseDeletableModel<string>
    {
        public Job()
        {
            this.Id = Guid.NewGuid().ToString();
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public DateTime StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

        public string TeamId { get; set; }

        public Team Team { get; set; }

        public int ServiceId { get; set; }

        public Service Service { get; set; }

        public int AddresId { get; set; }

        public Address Address { get; set; }
    }
}
