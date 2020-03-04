namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;

    public class DetailsCategoryViewModel : IMapFrom<CategoryServiceModel>
    {
        public DetailsCategoryViewModel()
        {
            this.ChildCategories = new HashSet<DetailsCategoryViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public IEnumerable<DetailsCategoryViewModel> ChildCategories { get; set; }
    }
}
