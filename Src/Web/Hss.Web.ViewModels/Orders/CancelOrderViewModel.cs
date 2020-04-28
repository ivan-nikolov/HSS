namespace Hss.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class CancelOrderViewModel : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Service Frequency")]
        public ServiceFrequency ServiceFrequency { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CancelOrderAddressViewModel Address { get; set; }

        [Display(Name = "Team")]
        public string TeamName { get; set; }

        [Display(Name = "Service")]
        public string ServiceName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, CancelOrderViewModel>()
                .ForMember(m => m.StartDate, e => e.MapFrom(o => o.Jobs.Where(j => j.JobStatus == JobStatus.InProgress).FirstOrDefault().StartDate))
                .ForMember(m => m.EndDate, e => e.MapFrom(o => o.Jobs.Where(j => j.JobStatus == JobStatus.InProgress).FirstOrDefault().FinishDate))
                .ForMember(m => m.DayOfWeek, e => e.MapFrom(o => (DayOfWeek)o.Appointment.DayOfWeek));
        }
    }
}
