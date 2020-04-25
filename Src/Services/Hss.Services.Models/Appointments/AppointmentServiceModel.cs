namespace Hss.Services.Models.Appointments
{
    using System;

    using Hss.Data.Models.Enums;

    public class AppointmentServiceModel
    {
        public string Id { get; set; }

        public int WeekOfMonth { get; set; }

        public int DayOfWeek { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }

        public string TeamId { get; set; }
    }
}
