namespace Hss.Web.ViewModels.Administration.Categories
{
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class EditCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; }
    }
}
