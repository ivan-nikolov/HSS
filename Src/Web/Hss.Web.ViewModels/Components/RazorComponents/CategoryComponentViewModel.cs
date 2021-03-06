﻿namespace Hss.Web.ViewModels.Components.RazorComponents
{
    using Ganss.XSS;
    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class CategoryComponentViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public int? CurrentCategoryId => this.Id > 0 ? this.Id : (int?)null;

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string ShortDescription => this.Description.Length < 100 ? this.Description : this.Description.Substring(0, 100) + "...";

        public string SanitizeShortDescription => new HtmlSanitizer().Sanitize(this.ShortDescription);

        public string ParentCategoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public int ChildCategoriesCount { get; set; }

        public int ServicesCount { get; set; }
    }
}
