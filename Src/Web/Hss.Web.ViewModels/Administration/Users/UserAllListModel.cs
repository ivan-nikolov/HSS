namespace Hss.Web.ViewModels.Administration.Users
{
    using System.Collections.Generic;

    public class UserAllListModel
    {
        public UserAllListModel()
        {
            this.Users = new HashSet<UserAllViewModel>();
        }

        public IEnumerable<UserAllViewModel> Users { get; set; }
    }
}
