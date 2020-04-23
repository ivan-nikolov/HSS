namespace Hss.Web.ViewModels.Components.RazorComponents.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Hss.Data.Models.Enums;

    public class CreateOrderInputModel
    {
        public int AddressId { get; set; }

        public int ServiceId { get; set; }

        public bool IsRecurrent { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; } = ServiceFrequency.Once;

        public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;

        public string ClientId { get; set; }
    }
}
