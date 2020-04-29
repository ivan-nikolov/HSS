namespace Hss.Web.ViewModels.Components.RazorComponents.Dashboard.Jobs
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;
    using Hss.Web.ViewModels.Team.Jobs;

    public class JobsViewModel : IMapFrom<Job>
    {
        public JobStatus JobStatus { get; set; }

        public JobAddressViewModel OrderAddress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string OrderServiceName { get; set; }

        public string OrderTeamName { get; set; }
    }
}
