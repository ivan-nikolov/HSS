namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;

    public class DetailsCategoryViewModel : IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
