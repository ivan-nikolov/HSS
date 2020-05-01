namespace Hss.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Addresses;
    using Hss.Services.Data.CIties;
    using Hss.Services.Data.Countries;
    using Hss.Services.Models.Countries;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CountriesServiceTests : TestsBase
    {
        [Fact]
        public async Task CheckIfCountryExistsReturnsTrueIfCountryExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country = new Country()
            {
                Id = 1,
            };

            dbContext.Add(country);
            await dbContext.SaveChangesAsync();
            var result = await service.CheckIfCountryExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfCountryExistsReturnsFalseIfCountryDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country = new Country()
            {
                Id = 1,
            };

            dbContext.Add(country);
            await dbContext.SaveChangesAsync();
            var result = await service.CheckIfCountryExists(2);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            await service.CreateAsync("Country");
            var result = dbContext.Countries.FirstOrDefault(c => c.Name == "Country");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsyncWorksCorrectlyWhenCountryExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country = new Country()
            {
                Id = 1,
            };

            dbContext.Add(country);
            await dbContext.SaveChangesAsync();
            await service.DeleteAsync(1);

            var result = dbContext.Countries.Count();

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task DeleteAsyncReturnsWhenCountryDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country = new Country()
            {
                Id = 1,
            };

            dbContext.Add(country);
            await dbContext.SaveChangesAsync();
            await service.DeleteAsync(2);

            var result = dbContext.Countries.Count();

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetAllWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country1 = new Country()
            {
                Id = 1,
            };
            var country2 = new Country()
            {
                Id = 2,
            };
            var country3 = new Country()
            {
                Id = 3,
            };

            dbContext.Add(country1);
            dbContext.Add(country2);
            dbContext.Add(country3);
            await dbContext.SaveChangesAsync();

            var result = await service.GetAll();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllHavingCitiesWorksCorrectlyForCitiesWithoutServices()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country1 = new Country()
            {
                Id = 1,
            };
            var country2 = new Country()
            {
                Id = 2,
                Cities = new List<City>() { new City() },
            };
            var country3 = new Country()
            {
                Id = 3,
            };

            dbContext.Add(country1);
            dbContext.Add(country2);
            dbContext.Add(country3);
            await dbContext.SaveChangesAsync();

            var result = service.GetAllHavingCities();

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllHavingCitiesWorksCorrectlyForCitiesWithService()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country1 = new Country()
            {
                Id = 1,
            };
            var country2 = new Country()
            {
                Id = 2,
                Cities = new List<City>()
                {
                    new City()
                    {
                        Teams = new List<Team>()
                        {
                            new Team()
                            {
                                Services = new List<TeamService>()
                                {
                                    new TeamService(),
                                },
                            },
                        },
                    },
                },
            };
            var country3 = new Country()
            {
                Id = 3,
            };

            dbContext.Add(country1);
            dbContext.Add(country2);
            dbContext.Add(country3);
            await dbContext.SaveChangesAsync();

            var result = service.GetAllHavingCities(true);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsyncWorksCorrectlyWhenCountryExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country1 = new Country()
            {
                Id = 1,
            };
            var country2 = new Country()
            {
                Id = 2,
            };
            var country3 = new Country()
            {
                Id = 3,
            };

            dbContext.Add(country1);
            dbContext.Add(country2);
            dbContext.Add(country3);
            await dbContext.SaveChangesAsync();

            var result = await service.GetByIdAsync<CountryServiceModel>(2);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullIfCountryDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesServiceMock = new Mock<IAddressesService>();
            var citiesRepository = new EfDeletableEntityRepository<City>(dbContext);

            var repository = new EfDeletableEntityRepository<Country>(dbContext);
            var citiesServiceMock = new Mock<CitiesService>(citiesRepository, addressesServiceMock.Object);

            var service = new CountriesService(repository, citiesServiceMock.Object);
            var country1 = new Country()
            {
                Id = 1,
            };
            var country2 = new Country()
            {
                Id = 2,
            };
            var country3 = new Country()
            {
                Id = 3,
            };

            dbContext.Add(country1);
            dbContext.Add(country2);
            dbContext.Add(country3);
            await dbContext.SaveChangesAsync();

            var result = await service.GetByIdAsync<CountryServiceModel>(7);

            Assert.Null(result);
        }
    }
}
