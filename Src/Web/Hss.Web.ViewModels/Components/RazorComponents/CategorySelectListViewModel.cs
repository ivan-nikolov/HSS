namespace Hss.Web.ViewModels.Components.RazorComponents
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategorySelectListViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
