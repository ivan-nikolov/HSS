namespace Hss.Services.Data.Appointments
{
    using System;
    using System.Threading.Tasks;

    using Hss.Common;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Appointments;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IDeletableEntityRepository<Appointment> appointmentsRepository;

        public AppointmentsService(IDeletableEntityRepository<Appointment> appointmentsRepository)
        {
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task<AppointmentServiceModel> CreateAsync(DateTime date, int duration, string orderId)
        {
            var weekOfMonth = date.GetWeekOfMonth();

            var appointment = new Appointment()
            {
                OrderId = orderId,
                WeekOfMonth = weekOfMonth,
                StartDate = date,
                EndDate = date.AddHours(duration),
                DayOfWeek = (int)date.DayOfWeek,
            };

            await this.appointmentsRepository.AddAsync(appointment);
            await this.appointmentsRepository.SaveChangesAsync();

            return appointment.To<AppointmentServiceModel>();
        }
    }
}
