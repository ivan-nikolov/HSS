namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Service : BaseDeletableModel<int>
    {
        public Service()
        {
            this.Teams = new HashSet<TeamService>();
            this.Jobs = new HashSet<Job>();

            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public virtual ICollection<TeamService> Teams { get; set; }
    }
}
