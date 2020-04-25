namespace Hss.Services.Models.Orders
{
    using System;

    using Hss.Data.Models.Enums;

    public class OrderServiceModel
    {
        public int AddressId { get; set; }

        public int ServiceId { get; set; }

        public int ServiceDuration { get; set; }

        public bool IsRecurrent { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string ClientId { get; set; }
    }
}
