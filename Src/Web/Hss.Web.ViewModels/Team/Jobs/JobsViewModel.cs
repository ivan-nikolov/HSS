﻿namespace Hss.Web.ViewModels.Team.Jobs
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class JobsViewModel : IMapFrom<Job>
    {
        public string Id { get; set; }

        public JobStatus JobStatus { get; set; }

        public JobAddressViewModel OrderAddress { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string OrderServiceName { get; set; }

        public string OrderId { get; set; }
    }
}
