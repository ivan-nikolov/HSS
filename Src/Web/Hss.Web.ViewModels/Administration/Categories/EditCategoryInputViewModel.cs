namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;

    public class EditCategoryInputViewModel
    {
        [Required]
        [StringLength(
            ValidationConstants.CategiryNameMaxLength,
            MinimumLength = ValidationConstants.CategiryNameMinLength,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Descirption { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
