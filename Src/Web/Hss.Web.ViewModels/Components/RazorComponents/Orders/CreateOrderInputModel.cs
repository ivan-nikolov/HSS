namespace Hss.Web.ViewModels.Components.RazorComponents.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Hss.Data.Models.Enums;
    using Hss.Web.Infrastructure.Attributes;

    public class CreateOrderInputModel
    {
        public int AddressId { get; set; }

        public int ServiceId { get; set; }

        public int ServiceDuration { get; set; }

        public bool IsRecurrent { get; set; }

        [EnumDataType(typeof(ServiceFrequency))]
        public ServiceFrequency ServiceFrequency { get; set; } = ServiceFrequency.Once;

        [DateValidation(errorMessage: "Date must not be in the past. Date must be a weekday.")]
        public DateTime AppointmentDate { get; set; } = DateTime.UtcNow;

        public string ClientId { get; set; }
    }
}
