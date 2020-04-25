namespace Hss.Services.Models.Jobs
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Orders;

    public class JobServiceModel : IMapFrom<Job>, IMapTo<Job>
    {
        public string Id { get; set; }

        public JobStatus JobStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string OrderId { get; set; }

        public OrderServiceModel Order { get; set; }
    }
}
