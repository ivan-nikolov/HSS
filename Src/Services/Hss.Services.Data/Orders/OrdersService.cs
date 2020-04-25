namespace Hss.Services.Data.Orders
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Services.Data.Appointments;
    using Hss.Services.Data.Invoices;
    using Hss.Services.Data.JobsService;
    using Hss.Services.Data.Services;
    using Hss.Services.Models.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IDeletableEntityRepository<Order> ordersRepository;
        private readonly IJobsService jobsService;
        private readonly IInvoicesService invoicesService;
        private readonly IServicesService servicesService;

        public OrdersService(
            IDeletableEntityRepository<Order> ordersRepository,
            IAppointmentsService appointmentsService,
            IJobsService jobsService,
            IInvoicesService invoicesService,
            IServicesService servicesService)
        {
            this.ordersRepository = ordersRepository;
            this.AppointmentsService = appointmentsService;
            this.jobsService = jobsService;
            this.invoicesService = invoicesService;
            this.servicesService = servicesService;
        }

        public IAppointmentsService AppointmentsService { get; }

        public async Task CreateAsync(OrderServiceModel input)
        {
            var billingFrequency = BillingFrequency.Once;
            if (input.ServiceFrequency != ServiceFrequency.Once)
            {
                billingFrequency = BillingFrequency.Monthly;
            }

            var order = new Order()
            {
                ServiceId = input.ServiceId,
                Status = OrderStatus.Pending,
                ClientId = input.ClientId,
                AddresId = input.AddressId,
                BillingFrequency = billingFrequency,
                ServiceFrequency = input.ServiceFrequency,
            };

            await this.ordersRepository.AddAsync(order);
            await this.ordersRepository.SaveChangesAsync();

            var appointment = await this.AppointmentsService.CreateAsync(input.AppointmentDate, input.ServiceDuration, order.Id);
            order.AppointmetnId = appointment.Id;

            await this.jobsService.CreateAsync(order.Id, order.ServiceFrequency, appointment.StartDate, appointment.EndDate);

            if (input.IsRecurrent || order.BillingFrequency == BillingFrequency.Once)
            {
                await this.invoicesService.CreateAsync(order.Id, order.ClientId, order.ServiceId, order.ServiceFrequency, order.AddresId);
            }
        }

        public int GetTotalMontJobsTime(int serviceId, DateTime appointmentDate, string teamId = null)
        {
            var daysInMonth = DateTime.DaysInMonth(appointmentDate.Year, appointmentDate.Month);
            var daysLeft = daysInMonth - appointmentDate.Day;
            var serviceDuration = this.servicesService.GetServiceDuration(serviceId);
            var orders = this.ordersRepository.All()
                .Where(o => o.ServiceId == serviceId && o.Status == OrderStatus.InProgress);
            if (teamId != null)
            {
                orders = orders.Where(o => o.TeamId == teamId);
            }

            var totalTime = 0;

            totalTime += this.GetDoneJobsTotalTime(serviceId, appointmentDate, serviceDuration);
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

        private int GetDoneJobsTotalTime(int serviceId, DateTime appointmentDate, int serviceDuration)
            => this.ordersRepository.All()
                            .Where(o => o.ServiceId == serviceId)
                            .SelectMany(o => o.Jobs)
                            .Where(j => j.StartDate.Day > 1 && j.StartDate.Day < appointmentDate.Day)
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
