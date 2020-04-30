﻿namespace Hss.Services.Notifier.EventArgs
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Mapping;

    public class CreateOrderEventArgs : IMapFrom<Order>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public ServiceFrequency ServiceFrequency { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public AddressEventArgs Address { get; set; }

        public OrderStatus Status { get; set; }

        public string TeamName { get; set; }

        public string ServiceName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Order, CreateOrderEventArgs>()
                .ForMember(m => m.StartDate, e => e.MapFrom(o => o.Jobs.Where(j => j.JobStatus == JobStatus.InProgress).FirstOrDefault().StartDate))
                .ForMember(m => m.EndDate, e => e.MapFrom(o => o.Jobs.Where(j => j.JobStatus == JobStatus.InProgress).FirstOrDefault().FinishDate))
                .ForMember(m => m.DayOfWeek, e => e.MapFrom(o => (DayOfWeek)o.Appointment.DayOfWeek));
        }
    }
}
