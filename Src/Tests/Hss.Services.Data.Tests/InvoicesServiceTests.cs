namespace Hss.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Models.Enums;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Invoices;
    using Hss.Services.Data.Services;
    using Hss.Services.Models.Invoices;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class InvoicesServiceTests : TestsBase
    {
        [Fact]
        public async Task CreateAsyncWorksCorrectlyForSingleOrders()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var servicesRepository = new EfDeletableEntityRepository<Service>(dbContext);
            var servicesServiceMock = new Mock<IServicesService>();
            servicesServiceMock.Setup(s => s.GetServicePrice(1)).Returns(100);

            var repository = new EfDeletableEntityRepository<Invoice>(dbContext);

            var service = new InvoicesService(repository, servicesServiceMock.Object);
            await service.CreateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1, ServiceFrequency.Once, 1, 1);
            var result = dbContext.Invoices.FirstOrDefault();

            var expectedNetAmount = 100;
            var expectedTotalAmoount = 120;

            Assert.Equal(expectedNetAmount, result.NetAmount);
            Assert.Equal(expectedTotalAmoount, result.TotalAmount);
        }

        [Fact]
        public async Task CreateAsyncWorksCorrectlyForReccurentOrders()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var servicesRepository = new EfDeletableEntityRepository<Service>(dbContext);
            var servicesServiceMock = new Mock<IServicesService>();
            servicesServiceMock.Setup(s => s.GetServicePrice(1)).Returns(100);

            var repository = new EfDeletableEntityRepository<Invoice>(dbContext);

            var service = new InvoicesService(repository, servicesServiceMock.Object);
            await service.CreateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 1, ServiceFrequency.Monthly, 1, 1);
            var result = dbContext.Invoices.FirstOrDefault();

            var expectedNetAmount = 95;
            var expectedTotalAmoount = 114;

            Assert.Equal(expectedNetAmount, result.NetAmount);
            Assert.Equal(expectedTotalAmoount, result.TotalAmount);
        }

        [Theory]
        [MemberData(nameof(GetByClientIdTestData))]
        public async Task GetByClientIdWorksCorrectly(string beforeDate, string afterDate, bool orderedByDateDesc, InvoiceStatus status, List<InvoiceServiceModel> expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var servicesRepository = new EfDeletableEntityRepository<Service>(dbContext);
            var servicesServiceMock = new Mock<IServicesService>();

            var repository = new EfDeletableEntityRepository<Invoice>(dbContext);

            var service = new InvoicesService(repository, servicesServiceMock.Object);

            var clientId = Guid.NewGuid().ToString();
            var before = DateTime.ParseExact(beforeDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var after = DateTime.ParseExact(afterDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var invoice1 = new Invoice()
            {
                ClientId = "1",
                CreatedOn = new DateTime(2020, 4, 15),
                Status = InvoiceStatus.Paid,
            };

            var invoice2 = new Invoice()
            {
                ClientId = "1",
                CreatedOn = new DateTime(2020, 4, 27),
                Status = InvoiceStatus.Paid,
            };

            var invoice3 = new Invoice()
            {
                ClientId = "1",
                CreatedOn = new DateTime(2020, 4, 27),
                Status = InvoiceStatus.Pending,
            };

            var invoice4 = new Invoice()
            {
                ClientId = "1",
                CreatedOn = new DateTime(2020, 4, 27),
                Status = InvoiceStatus.Cancelled,
            };

            var invoice5 = new Invoice()
            {
                ClientId = Guid.NewGuid().ToString(),
                CreatedOn = new DateTime(2020, 1, 27),
                Status = InvoiceStatus.Cancelled,
            };

            dbContext.Add(invoice1);
            dbContext.Add(invoice2);
            dbContext.Add(invoice3);
            dbContext.Add(invoice4);
            dbContext.Add(invoice5);
            await dbContext.SaveChangesAsync();

            var services = service.GetByClientId<InvoiceServiceModel>("1", before, after, orderedByDateDesc, status).ToList();
            var expectedList = expected.ToList();

            Assert.Equal(expectedList.Count, services.Count);

            for (int i = 0; i < services.Count; i++)
            {
                Assert.Equal(expectedList[i].CreatedOn, services[i].CreatedOn);
                Assert.Equal(expectedList[i].Status, services[i].Status);
            }
        }

        public static IEnumerable<object[]> GetByClientIdTestData()
            => new List<object[]>
            {
                new object[]
                {
                    "2020-04-27",
                    "2020-03-27",
                    false,
                    InvoiceStatus.Paid,
                    new List<InvoiceServiceModel>()
                    {
                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 15),
                            Status = InvoiceStatus.Paid,
                        },

                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 27),
                            Status = InvoiceStatus.Paid,
                        },
                    },
                },
                new object[]
                {
                    "2020-04-27",
                    "2020-03-27",
                    true,
                    InvoiceStatus.Paid,
                    new List<InvoiceServiceModel>()
                    {

                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 27),
                            Status = InvoiceStatus.Paid,
                        },
                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 15),
                            Status = InvoiceStatus.Paid,
                        },
                    },
                },
                new object[]
                {
                    "2020-04-27",
                    "2020-04-16",
                    false,
                    InvoiceStatus.Paid,
                    new List<InvoiceServiceModel>()
                    {
                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 27),
                            Status = InvoiceStatus.Paid,
                        },
                    },
                },
                new object[]
                {
                    "2020-04-26",
                    "2020-03-27",
                    false,
                    InvoiceStatus.Paid,
                    new List<InvoiceServiceModel>()
                    {
                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 15),
                            Status = InvoiceStatus.Paid,
                        },
                    },
                },
                new object[]
                {
                    "2020-04-27",
                    "2020-03-27",
                    false,
                    InvoiceStatus.Cancelled,
                    new List<InvoiceServiceModel>()
                    {
                        new InvoiceServiceModel()
                        {
                            ClientId = "1",
                            CreatedOn = new DateTime(2020, 4, 27),
                            Status = InvoiceStatus.Cancelled,
                        },
                    },
                },
            };
    }
}
