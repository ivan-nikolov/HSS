namespace Hss.Services.Data.Appointments
{
    using System;
    using System.Threading.Tasks;

    using Hss.Services.Models.Appointments;

    public interface IAppointmentsService
    {
        Task<AppointmentServiceModel> CreateAsync(DateTime date, int duration, string orderId);
    }
}
