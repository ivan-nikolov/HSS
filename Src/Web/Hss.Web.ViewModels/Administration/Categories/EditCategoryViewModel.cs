namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditCategoryViewModel : IMapFrom<CategoryServiceModel>
    {
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParantCategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
