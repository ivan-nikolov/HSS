namespace Hss.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AssignUserToTeamInputModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string TeamId { get; set; }

        public IEnumerable<SelectListItem> Teams { get; set; }
    }
}
