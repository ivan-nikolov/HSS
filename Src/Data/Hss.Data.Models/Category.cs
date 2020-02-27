namespace Hss.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Hss.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.ChildCategories = new HashSet<Category>();
            this.Services = new HashSet<Service>();

            this.IsDeleted = false;
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public int ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }

        public virtual ICollection<Category> ChildCategories { get; set; }

        public virtual ICollection<Service> Services { get; set; }
    }
}
