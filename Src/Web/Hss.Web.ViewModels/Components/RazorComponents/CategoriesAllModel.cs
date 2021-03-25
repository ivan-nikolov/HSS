namespace Hss.Web.ViewModels.Components.RazorComponents
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoriesAllModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public int? CurrentCategoryId => this.Id > 0 ? this.Id : (int?)null;

        public int? ParentCategoryId { get; set; }

        public string Name { get; set; }

        public string ParentCategoryName { get; set; }
    }
}
