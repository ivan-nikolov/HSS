namespace Hss.Services.Models.Appointments
{
    using System;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class AppointmentServiceModel : IMapFrom<Appointment>, IMapTo<Appointment>
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
