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
            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;

            this.TeamMembers = new HashSet<ApplicationUser>();
            this.Services = new HashSet<TeamService>();
            this.Jobs = new HashSet<Job>();
        }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> TeamMembers { get; set; }

        public virtual ICollection<TeamService> Services { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }
    }
}
