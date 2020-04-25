namespace Hss.Web.ViewModels.Components.RazorComponents.Dashboard.Addresses
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Services.Mapping;
    using Hss.Services.Models.Addresses;
    using Hss.Web.ViewModels.Common;

    public class AddressInputModel : IMapTo<AddressServiceModel>
    {
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        [Display(Name = "Building Number")]
        public string BuildingNumber { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Appartment { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        [Display(Name = "Street")]
        public string StreetName { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Neighborhood { get; set; }

        public string PostCode { get; set; }

        public int CityId { get; set; }

        public int CountryId { get; set; }
    }
}
