namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using System.ComponentModel.DataAnnotations;

    using Hss.Web.ViewModels.Common;

    public class CreateCountryInputModel
    {
        [Required]
        [StringLength(90, MinimumLength = 4, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }
    }
}
