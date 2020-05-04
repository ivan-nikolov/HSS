namespace Hss.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Data.Repositories;
    using Hss.Services.Data.DateTime;
    using Hss.Services.Data.Teams;
    using Hss.Services.Models.Teams;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class TeamsServiceTests : TestsBase
    {
        [Theory]
        [MemberData(nameof(GetFreeTeamsTestParams))]
        public void GetFreeTeamsWorksCorrectly(ServiceFrequency appointmentServiceFrequency, DateTime appointmentStartDate, DateTime appointmentEndDate, ServiceFrequency serviceFrequency,DateTime startDate, DateTime endDate, int expectedCount)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repositoryMock = new Mock<IDeletableEntityRepository<Team>>();
            repositoryMock.Setup(x => x.All())
                .Returns(new List<Team>()
                                    {
                                        new Team()
                                        {
                                            Id = "1",
                                            CityId = 1,
                                            Services = new List<TeamService>()
                                            {
                                                new TeamService()
                                                {
                                                    TeamId = "1",
                                                    ServiceId = 1,
                                                },
                                            },
                                            Orders = new List<Order>()
                                            {
                                                new Order()
                                                {
                                                    Id = "1",
                                                    ServiceFrequency = appointmentServiceFrequency,
                                                    Status = OrderStatus.Pending,
                                                    AppointmetnId = "1",
                                                    TeamId = "1",
                                                    Appointment = new Appointment()
                                                        {
                                                            Id = "1",
                                                            OrderId = "1",
                                                            DayOfWeek = (int)appointmentStartDate.DayOfWeek,
                                                            StartDate = appointmentStartDate,
                                                            EndDate = appointmentEndDate,
                                                        },
                                                },
                                            },
                                            TeamMembers = new List<ApplicationUser>()
                                            {
                                                new ApplicationUser()
                                                {
                                                    Id = "1",
                                                    TeamId = "1",
                                                },
                                            },
                                        },
                                    }.AsQueryable());

            var service = new TeamsService(repositoryMock.Object);

            var result = service.GetFreeTeams<TeamServiceModel>(startDate, endDate, 1, 1, serviceFrequency);

            Assert.Equal(expectedCount, result.Count());
        }

        public static IEnumerable<object[]> GetFreeTeamsTestParams()
            => new List<object[]>()
            {
                new object[]
                {
                    ServiceFrequency.Once,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),

                    ServiceFrequency.Once,
                    new DateTime(2020, 5, 5, 10, 0, 0),
                    new DateTime(2020, 5, 5, 12, 0, 0),
                    1,
                },

                new object[]
                {
                    ServiceFrequency.Daily,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),

                    ServiceFrequency.Daily,
                    new DateTime(2020, 5, 5, 10, 0, 0),
                    new DateTime(2020, 5, 5, 12, 0, 0),
                    0,
                },

                new object[]
                {
                    ServiceFrequency.Daily,
                    new DateTime(2020, 5, 5, 10, 0, 0),
                    new DateTime(2020, 5, 5, 12, 0, 0),

                    ServiceFrequency.Daily,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),
                    0,
                },

                new object[]
                {
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),

                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 12, 10, 0, 0),
                    new DateTime(2020, 5, 12, 12, 0, 0),
                    1,
                },

                new object[]
                {
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),

                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),
                    0,
                },

                new object[]
                {
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),
                    0,
                },

                new object[]
                {
                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    ServiceFrequency.Once,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),
                    1,
                },

                new object[]
                {
                    ServiceFrequency.Monthly,
                    new DateTime(2020, 5, 11, 10, 0, 0),
                    new DateTime(2020, 5, 11, 12, 0, 0),

                    ServiceFrequency.Weekly,
                    new DateTime(2020, 5, 4, 10, 0, 0),
                    new DateTime(2020, 5, 4, 12, 0, 0),
                    0,
                },
            };
    }
}
