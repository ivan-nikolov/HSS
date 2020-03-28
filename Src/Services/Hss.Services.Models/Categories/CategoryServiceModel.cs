namespace Hss.Services.Models.Categories
{
    using System.Collections.Generic;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoryServiceModel : IMapTo<Category>, IMapFrom<Category>
    {
        public CategoryServiceModel()
        {
            this.ChildCategories = new HashSet<CategoryServiceModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; }

        public virtual ICollection<CategoryServiceModel> ChildCategories { get; set; }
    }
}
