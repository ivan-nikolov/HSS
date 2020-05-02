namespace Hss.Web.ViewModels.Administration.Services
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;
    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Http;

    public class CreateServiceInputModel : IMapTo<ServiceServiceModel>, IHaveCustomMappings
    {
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

        public int ParentCategoryId { get; set; }

        [Display(Name = "Recurrent Service")]
        public bool IsRecurrent { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<CreateServiceInputModel, ServiceServiceModel>()
                .ForMember(s => s.CategoryId, p => p.MapFrom(e => e.ParentCategoryId));
        }
    }
}
