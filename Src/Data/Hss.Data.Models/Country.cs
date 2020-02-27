namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Country : BaseDeletableModel<int>
    {
        public Country()
        {
            this.Cities = new HashSet<City>();
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
