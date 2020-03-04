namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Categories;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateCategoryInputModel : IMapTo<CategoryServiceModel>
    {
        [Required]
        [StringLength(
            ValidationConstants.CategiryNameMaxLength,
            MinimumLength = ValidationConstants.CategiryNameMinLength,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
