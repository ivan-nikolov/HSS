﻿namespace Hss.Services.Data.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Data.Appointments;
    using Hss.Services.Data.Invoices;
    using Hss.Services.Data.JobsService;
    using Hss.Services.Data.Services;
    using Hss.Services.Data.Teams;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Orders;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IJobsService jobsService;
        private readonly IInvoicesService invoicesService;
        private readonly IServicesService servicesService;
        private readonly ITeamsService teamsService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IAppointmentsService appointmentsService,
            IJobsService jobsService,
            IInvoicesService invoicesService,
            IServicesService servicesService,
            ITeamsService teamsService)
        {
            this.ordersRepository = ordersRepository;
            this.AppointmentsService = appointmentsService;
            this.jobsService = jobsService;
            this.invoicesService = invoicesService;
            this.servicesService = servicesService;
            this.teamsService = teamsService;
        }

        public IAppointmentsService AppointmentsService { get; }

        public async Task CreateAsync(OrderServiceModel input)
        {
            await this.ordersRepository.SaveChangesAsync();
            var billingFrequency = BillingFrequency.Once;
            if (input.ServiceFrequency != ServiceFrequency.Once)
            {
                billingFrequency = BillingFrequency.Monthly;
            }

            var totalOrdersTime = this.GetTotalMonthJobsTime(input.ServiceId, input.AppointmentDate);
            var teamId = this.teamsService
                .GetFreeTeams(input.AppointmentDate, input.AppointmentDate.AddHours(input.ServiceDuration), input.CityId, input.ServiceId)
                .OrderBy(t => this.GetTotalMonthJobsTime(input.ServiceId, input.AppointmentDate, t))
                .ToList();

            var order = new Order()
            {
                ServiceId = input.ServiceId,
                Status = OrderStatus.Pending,
                ClientId = input.ClientId,
                AddresId = input.AddressId,
                BillingFrequency = billingFrequency,
                ServiceFrequency = input.ServiceFrequency,
                TeamId = teamId.FirstOrDefault(),
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var appointment = await this.AppointmentsService.CreateAsync(input.AppointmentDate, input.ServiceDuration, order.Id);
            order.AppointmetnId = appointment.Id;

            await this.jobsService.CreateAsync(order.Id, order.ServiceFrequency, appointment.StartDate, appointment.EndDate);

            if (!input.IsRecurrent || order.BillingFrequency == BillingFrequency.Once)
            {
                var jobsCount = this.ordersRepository.All()
                    .Where(o => o.Id == order.Id)
                    .SelectMany(o => o.Jobs)
                    .Where(j => j.JobStatus == JobStatus.InProgress)
                    .Count();
                await this.invoicesService.CreateAsync(order.Id, order.ClientId, order.ServiceId, order.ServiceFrequency, order.AddresId, jobsCount);
            }
        }

        public async Task<IEnumerable<T>> GetActiveOrdersByUserId<T>(string userId)
            => await this.ordersRepository
            .All()
            .Where(o => o.Status == OrderStatus.Pending && o.ClientId == userId)
            .OrderByDescending(o => o.CreatedOn)
            .To<T>()
            .ToListAsync();

        public IEnumerable<T> GetAllWithUnpaidJobs<T>()
            => this.ordersRepository.All()
            .Where(o => o.Jobs.Any(j => j.JobStatus == JobStatus.Done))
            .To<T>();

        public int GetTotalMonthJobsTime(int serviceId, DateTime appointmentDate, string teamId = null)
        {
            var daysInMonth = DateTime.DaysInMonth(appointmentDate.Year, appointmentDate.Month);
            var daysLeft = daysInMonth - appointmentDate.Day;
            var serviceDuration = this.servicesService.GetServiceDuration(serviceId);
            var orders = this.ordersRepository.All()
                .Where(o => o.ServiceId == serviceId);
            if (teamId != null)
            {
                orders = orders.Where(o => o.TeamId == teamId);
            }

            var totalTime = 0;

            totalTime += this.GetDoneJobsTotalTime(serviceId, appointmentDate, serviceDuration, orders);
            totalTime += this.GetSingleOrdersTotalTime(appointmentDate, serviceDuration, orders);
            totalTime += this.GetDailyOrdersTotalTime(daysLeft, orders);
            totalTime += this.GetWeeklyOrdersTotalTime(daysLeft, orders);
            totalTime += this.GetMonthlyOrdersTotalTime(appointmentDate, orders);

            return totalTime;
        }

        public int GetUnpaidRecurrentJobsCount(string id)
            => this.ordersRepository
            .All()
            .Where(o => o.Id == id && o.Service.IsRecurrent && o.ServiceFrequency != ServiceFrequency.Once)
            .SelectMany(o => o.Jobs)
            .Where(j => j.JobStatus == JobStatus.Done)
            .Count();

        private int GetDoneJobsTotalTime(int serviceId, DateTime appointmentDate, int serviceDuration, IQueryable<Order> orders)
            => orders
                            .SelectMany(o => o.Jobs)
                            .Where(j => j.StartDate.Day > 1
                            && j.StartDate.Month <= appointmentDate.Month
                            && j.StartDate.Year <= appointmentDate.Year
                            && j.StartDate < appointmentDate)
                            .Count() * serviceDuration;

        private int GetSingleOrdersTotalTime(DateTime appointmentDate, int serviceDuration, IQueryable<Order> orders)
            => orders
                .Where(o => o.ServiceFrequency == ServiceFrequency.Once
                    && o.Appointment.StartDate >= appointmentDate
                    && o.Appointment.StartDate.Month == appointmentDate.Month
                    && o.Appointment.StartDate.Year == appointmentDate.Year)
                .Count() * serviceDuration;

        private int GetDailyOrdersTotalTime(int daysLeft, IQueryable<Order> orders)
            => orders
                .Where(o => o.ServiceFrequency == ServiceFrequency.Daily)
                .Take(daysLeft)
                .Sum(o => o.Service.DurationInHours);

        private int GetWeeklyOrdersTotalTime(int daysLeft, IQueryable<Order> orders)
            => orders
                .Where(o => o.ServiceFrequency == ServiceFrequency.Weekly)
                .Take(daysLeft / 7)
                .Sum(o => o.Service.DurationInHours);

        private int GetMonthlyOrdersTotalTime(DateTime appointmentDate, IQueryable<Order> orders)
            => orders
                .Where(o => o.ServiceFrequency == ServiceFrequency.Monthly
                    && o.Appointment.StartDate.Day > appointmentDate.Day
                    && o.Appointment.StartDate.Year <= appointmentDate.Year
                    && o.Appointment.StartDate.Month <= appointmentDate.Month)
                .Sum(o => o.Service.DurationInHours);
    }
}