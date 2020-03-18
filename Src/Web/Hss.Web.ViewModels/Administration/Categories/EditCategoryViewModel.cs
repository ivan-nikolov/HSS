namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class EditCategoryViewModel : IMapFrom<CategoryServiceModel>
    {
        public string Name { get; set; }

        public string Image { get; set; }

        public string Descirption { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParantCategoryName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
