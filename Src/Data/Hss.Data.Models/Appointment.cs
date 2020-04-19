namespace Hss.Data.Models
{
    using System;

    using Hss.Data.Common.Models;
    using Hss.Data.Models.Enums;

    public class Appointment : BaseDeletableModel<string>
    {
        public Appointment()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int WeekOfMonth { get; set; }

        public int DayOfWeek { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }
    }
}
