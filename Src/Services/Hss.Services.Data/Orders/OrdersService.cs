namespace Hss.Services.Data.Orders
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
    using Hss.Services.Models.Teams;
    using Microsoft.EntityFrameworkCore;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IAppointmentsService appointmentsService;
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
            this.appointmentsService = appointmentsService;
            this.appointmentsService = appointmentsService;
            this.jobsService = jobsService;
            this.invoicesService = invoicesService;
            this.servicesService = servicesService;
            this.teamsService = teamsService;
        }

        public async Task CancelAsync(string id)
        {
            var order = await this.ordersRepository.All()
                .FirstAsync(o => o.Id == id);
            if (order.Status != OrderStatus.InProgress && order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Order can not be cancelled!");
            }

            order.Status = OrderStatus.Cancelled;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();

            await this.jobsService.CancelByOrderIdAsync(id);
        }

        public async Task CompleteAsync(string id)
        {
            var order = await this.ordersRepository.All()
                .FirstOrDefaultAsync(o => o.Id == id);

            order.Status = OrderStatus.Done;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public async Task<string> CreateAsync(OrderServiceModel input)
        {
            var billingFrequency = input.ServiceFrequency == ServiceFrequency.Once ? BillingFrequency.Once : BillingFrequency.Monthly;

            var teamId = this.teamsService
                .GetFreeTeams<TeamServiceModel>(input.AppointmentDate, input.AppointmentDate.AddHours(input.ServiceDuration), input.CityId, input.ServiceId, input.ServiceFrequency)
                .OrderBy(t => this.GetTotalMonthJobsTime(input.ServiceId, input.AppointmentDate, t.Id))
                .FirstOrDefault().Id;

            var order = new Order()
            {
                ServiceId = input.ServiceId,
                Status = OrderStatus.Pending,
                ClientId = input.ClientId,
                AddresId = input.AddressId,
                BillingFrequency = billingFrequency,
                ServiceFrequency = input.ServiceFrequency,
                TeamId = teamId,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var appointment = await this.appointmentsService.CreateAsync(input.AppointmentDate, input.ServiceDuration, order.Id);
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

            return order.Id;
        }

        public async Task ConfirmAsync(string id, string teamId)
        {
            var order = this.ordersRepository.All()
                .Where(o => o.Id == id)
                .FirstOrDefault();

            order.TeamId = teamId;
            order.Status = OrderStatus.InProgress;
            this.ordersRepository.Update(order);
            await this.ordersRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetPendingOrders<T>()
            => this.ordersRepository.All()
            .Where(o => o.Status == OrderStatus.Pending)
            .OrderBy(o => o.Appointment.StartDate)
            .To<T>();

        public bool CheckIfOrderExists(string id)
            => this.ordersRepository.All()
            .Any(o => o.Id == id);

        public IQueryable<T> GetOrdersByUserId<T>(string userId)
            => this.ordersRepository
            .All()
            .Where(o => o.ClientId == userId)
            .OrderByDescending(o => o.CreatedOn)
            .To<T>();

        public IEnumerable<T> GetAllWithUnpaidJobs<T>()
            => this.ordersRepository.All()
            .Where(o => o.ServiceFrequency != ServiceFrequency.Once && o.Jobs.Any(j => j.JobStatus == JobStatus.Done))
            .To<T>();

        public T GetById<T>(string id)
            => this.ordersRepository.All()
            .Where(o => o.Id == id)
            .To<T>()
            .FirstOrDefault();

        public async Task<T> GetByIdAsync<T>(string id)
            => await this.ordersRepository.All()
            .Where(o => o.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

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

            totalTime += this.GetDoneJobsTotalTime(appointmentDate, serviceDuration, orders);
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

        private int GetDoneJobsTotalTime(DateTime appointmentDate, int serviceDuration, IQueryable<Order> orders)
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
