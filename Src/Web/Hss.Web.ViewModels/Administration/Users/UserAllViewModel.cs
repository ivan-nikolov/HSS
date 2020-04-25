namespace Hss.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class UserAllViewModel : IMapFrom<ApplicationUser>
    {
        public UserAllViewModel()
        {
            this.UserRoles = new HashSet<string>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
    }
}
