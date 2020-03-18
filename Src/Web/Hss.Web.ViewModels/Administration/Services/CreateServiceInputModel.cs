namespace Hss.Web.ViewModels.Administration.Services
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateServiceInputModel : IMapTo<ServiceServiceModel>
    {
        public CreateServiceInputModel()
        {
            this.Categories = new List<SelectListItem>();
        }

        [Required]
        [StringLength(
            ValidationConstants.ServiceNamehMaxLength,
            MinimumLength = ValidationConstants.ServiceNameMinLength,
            ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        [Range(typeof(decimal), "0.00", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
