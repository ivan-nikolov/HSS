namespace Hss.Web.ViewModels.Administration.Categories
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class DetailsCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
