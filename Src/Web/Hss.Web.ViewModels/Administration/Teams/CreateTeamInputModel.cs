namespace Hss.Web.ViewModels.Administration.Teams
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Hss.Web.ViewModels.Common;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CreateTeamInputModel
    {
        [Required]
        [StringLength(ValidationConstants.TeamNameMaxLength, MinimumLength = ValidationConstants.TeamNameMinLength, ErrorMessage = ValidationConstants.StringLengthErrorMessage)]
        public string Name { get; set; }

        public int CityId { get; set; }

        public IEnumerable<int> Services { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        public IEnumerable<SelectListItem> AllServices { get; set; }
    }
}
