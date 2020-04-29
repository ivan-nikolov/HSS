namespace Hss.Web.ViewModels.Team.Jobs
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class OrderInputModel : IMapFrom<Order>
    {
        public ServiceFrequency ServiceFrequency { get; set; }

        public DateTime AppointmentStartDate { get; set; }

        public DateTime AppointmentEndDate { get; set; }
    }
}
