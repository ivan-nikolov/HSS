namespace Hss.Web.ViewModels.Administration.Services
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;

    public class EditServiceInputModel : IMapTo<ServiceServiceModel>, IMapFrom<Service>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(
            ValidationConstants.ServiceNamehMaxLength,
            MinimumLength = ValidationConstants.ServiceNameMinLength,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}
