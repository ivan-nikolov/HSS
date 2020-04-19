namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Team : BaseDeletableModel<string>
    {
        public Team()
        {
            this.Id = Guid.NewGuid().ToString();
            this.TeamMembers = new HashSet<ApplicationUser>();
            this.Services = new HashSet<TeamService>();
            this.Orders = new HashSet<Order>();
        }

        public string Name { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public virtual ICollection<ApplicationUser> TeamMembers { get; set; }

        public virtual ICollection<TeamService> Services { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
