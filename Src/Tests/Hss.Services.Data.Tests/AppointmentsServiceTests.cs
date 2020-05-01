namespace Hss.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Addresses;
    using Hss.Services.Data.Appointments;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AppointmentsServiceTests : TestsBase
    {
        [Fact]
        public async Task CreateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<Appointment>(dbContext);

            var service = new AppointmentsService(repository);

            var orderId = Guid.NewGuid().ToString();
            var startDate = new DateTime(2020, 4, 27, 10, 0, 0);
            var endDate = new DateTime(2020, 4, 27, 12, 0, 0);

            await service.CreateAsync(startDate, 2, orderId);

            var result = dbContext.Appointments.FirstOrDefault();

            Assert.Equal(result.EndDate, endDate);
        }
    }
}
