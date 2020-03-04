namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Web.ViewModels.Common;

    public class EditCategoryInputViewModel
    {
        [Required]
        [StringLength(
            ValidationConstants.CategiryNameMaxLength,
            MinimumLength = ValidationConstants.CategiryNameMinLength,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
    }
}
