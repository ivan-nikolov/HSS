namespace Hss.Web.ViewModels.Administration.OperationalLocations
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateCityInputModel
    {
        [Required]
        [StringLength(200, MinimumLength = 2, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
