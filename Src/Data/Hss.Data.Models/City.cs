namespace Hss.Data.Models
{
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public City()
        {
            this.Addresses = new HashSet<Address>();
            this.Teams = new HashSet<Team>();
        }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
