namespace Hss.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;

    public class EditCategoryInputModel : IMapTo<Category>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(
            ValidationConstants.CategoryNameMaxLength,
            MinimumLength = ValidationConstants.CategoryNameMinLength,
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
