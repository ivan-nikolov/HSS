namespace Hss.Services.Models.Categories
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoryServiceModel : IMapTo<Category>, IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
