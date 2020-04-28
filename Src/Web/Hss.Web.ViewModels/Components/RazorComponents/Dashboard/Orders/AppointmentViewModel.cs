namespace Hss.Web.ViewModels.Components.RazorComponents.Dashboard.Orders
{
    using System;

    using Hss.Data.Models;
    using Hss.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>
    {
        public int DayOfWeek { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
