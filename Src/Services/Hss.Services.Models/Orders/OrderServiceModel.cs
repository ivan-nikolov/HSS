namespace Hss.Services.Models.Orders
{
    using System;
    using System.Collections.Generic;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Jobs;
    using Hss.Services.Models.Services;

    public class OrderServiceModel : IMapFrom<Order>, IMapTo<Order>
    {
        public OrderServiceModel()
        {
            this.Jobs = new HashSet<JobServiceModel>();
        }

        public string Id { get; set; }

        public int AddressId { get; set; }

        public int ServiceId { get; set; }

        public virtual ServiceServiceModel Service { get; set; }

        public int ServiceDuration { get; set; }

        public int CityId { get; set; }

        public bool IsRecurrent { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string ClientId { get; set; }

        public ICollection<JobServiceModel> Jobs { get; set; }
    }
}
