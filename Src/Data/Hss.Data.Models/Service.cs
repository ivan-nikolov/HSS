namespace Hss.Data.Models
{
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Service : BaseDeletableModel<int>
    {
        public Service()
        {
            this.Teams = new HashSet<TeamService>();
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsRecurrent { get; set; }

        public decimal Price { get; set; }

        public int DurationInHours { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<TeamService> Teams { get; set; }
    }
}
