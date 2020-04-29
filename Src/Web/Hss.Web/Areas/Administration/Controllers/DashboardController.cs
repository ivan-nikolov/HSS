namespace Hss.Web.Areas.Administration.Controllers
{
    using Hss.Services.Data;
    using Hss.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    // TODO:Delete if not needed
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }
    }
}
