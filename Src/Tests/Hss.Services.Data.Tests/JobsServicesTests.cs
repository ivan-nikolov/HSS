namespace Hss.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Data.Repositories;
    using Hss.Services.Data.DateTime;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class JobsServicesTests : TestsBase
    {
        [Theory]
        [MemberData(nameof(GetCreateAsyncParams))]
        public async Task CreateAsyncWorksCorrectly(DateTime currentDate, ServiceFrequency serviceFrequency, DateTime appointmentStartDate, DateTime appointmentEndDate, DateTime expectedStartDate, DateTime expectedFinishDate)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(d => d.GetUtcNow()).Returns(currentDate);

            var repository = new EfDeletableEntityRepository<Job>(dbContext);

            var service = new JobsService.JobsService(repository, dateTimeProviderMock.Object);
            await service.CreateAsync(Guid.NewGuid().ToString(), serviceFrequency, appointmentStartDate, appointmentEndDate);
            var result = dbContext.Jobs.FirstOrDefault();

            Assert.Equal(expectedStartDate, result.StartDate);
            Assert.Equal(expectedFinishDate, result.FinishDate);
        }


        [Theory]
        [InlineData(ServiceFrequency.Once, 1)]
        [InlineData(ServiceFrequency.Weekly, 2)]
        public async Task CompleteJobAsyncWorksCorrectly(ServiceFrequency serviceFrequency, int expectedJobsCount)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var id = Guid.NewGuid().ToString();
            var job = new Job() 
            {
                Id = id,
            };
            dbContext.Add(job);
            await dbContext.SaveChangesAsync();

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(d => d.GetUtcNow()).Returns(new DateTime(2020, 5, 4, 12, 30, 0));
            var repository = new EfDeletableEntityRepository<Job>(dbContext);
            var service = new JobsService.JobsService(repository, dateTimeProviderMock.Object);

            await service.CompleteJobAsync(id, Guid.NewGuid().ToString(), serviceFrequency, new DateTime(2020, 5, 4, 10, 0, 0), new DateTime(2020, 5, 4, 12, 0, 0));
            var result = dbContext.Jobs.Count();

            Assert.Equal(expectedJobsCount, result);
        }

        public static IEnumerable<object[]> GetCreateAsyncParams()
            => new List<object[]>
            {
                // Initial create. Appointment date in future.
                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Once,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },
                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Daily,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },
                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },

                // Recurrent services next job create.
                new object[]
                {
                    new DateTime(2020, 05, 8, 12, 30, 0),
                    ServiceFrequency.Daily,
                    new DateTime(2020, 4, 6, 10, 0, 0),
                    new DateTime(2020, 4, 6, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 4, 6, 10, 0, 0),
                    new DateTime(2020, 4, 6, 12, 0, 0),

                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 05, 04, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 4, 6, 10, 0, 0),
                    new DateTime(2020, 4, 6, 12, 0, 0),

                    new DateTime(2020, 6, 1, 10, 0, 0),
                    new DateTime(2020, 6, 1, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 6, 1, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 4, 6, 10, 0, 0),
                    new DateTime(2020, 4, 6, 12, 0, 0),

                    new DateTime(2020, 7, 6, 10, 0, 0),
                    new DateTime(2020, 7, 6, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 9, 7, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 4, 6, 10, 0, 0),
                    new DateTime(2020, 4, 6, 12, 0, 0),

                    new DateTime(2020, 10, 5, 10, 0, 0),
                    new DateTime(2020, 10, 5, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 5, 28, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 4, 30, 10, 0, 0),
                    new DateTime(2020, 4, 30, 12, 0, 0),

                    new DateTime(2020, 6, 25, 10, 0, 0),
                    new DateTime(2020, 6, 25, 12, 0, 0),
                },

                new object[]
                {
                    new DateTime(2020, 6, 25, 12, 30, 0),
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 4, 30, 10, 0, 0),
                    new DateTime(2020, 4, 30, 12, 0, 0),

                    new DateTime(2020, 7, 30, 10, 0, 0),
                    new DateTime(2020, 7, 30, 12, 0, 0),
                },
            };
    }
}
