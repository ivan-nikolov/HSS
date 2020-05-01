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
    using Hss.Services.Models.Cities;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CitiesServiceTests : TestsBase
    {
        [Fact]
        public async Task CheckIfCityExistsAsyncReturnsTrueIfCityExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
            };

            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
            var result = await service.CheckIfCityExistsAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfCityExistsAsyncReturnsFalseIfCityDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
            };

            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
            var result = await service.CheckIfCityExistsAsync(2);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);

            await service.CreateAsync("1", 1);

            var result = dbContext.Cities.FirstOrDefault(c => c.Name == "1");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
            };

            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
            await service.DeleteAsync(1);

            var dbCity = dbContext.Cities.FirstOrDefault(c => c.Id == 1);

            Assert.Null(dbCity);
        }

        [Fact]
        public async Task GetByCountryIdWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
                CountryId = 1,
            };

            var city2 = new City()
            {
                Id = 2,
                CountryId = 1,
            };

            var city3 = new City()
            {
                Id = 3,
                CountryId = 2,
            };

            dbContext.Add(city);
            dbContext.Add(city2);
            dbContext.Add(city3);
            await dbContext.SaveChangesAsync();
            var cities = await service.GetByCountryIdAsync(1);

            Assert.Equal(2, cities.Count());
        }

        [Fact]
        public async Task GetByCountryIdGenericWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
                CountryId = 1,
            };

            var city2 = new City()
            {
                Id = 2,
                CountryId = 1,
            };

            var city3 = new City()
            {
                Id = 3,
                CountryId = 2,
            };

            dbContext.Add(city);
            dbContext.Add(city2);
            dbContext.Add(city3);
            await dbContext.SaveChangesAsync();
            var cities = await service.GetByCountryIdAsync<CityServiceModel>(1);

            Assert.Equal(2, cities.Count());
        }

        [Fact]
        public async Task GetByCountryIdWithTeamsWithServicesGenericWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
                CountryId = 1,
            };

            var city2 = new City()
            {
                Id = 2,
                CountryId = 1,
                Teams = new List<Team>() { new Team() { Services = new List<TeamService>() { new TeamService() } } },
            };

            var city3 = new City()
            {
                Id = 3,
                CountryId = 2,
            };

            dbContext.Add(city);
            dbContext.Add(city2);
            dbContext.Add(city3);
            await dbContext.SaveChangesAsync();
            var cities = await service.GetByCountryIdAsync(1, true);

            Assert.Single(cities);
        }

        [Fact]
        public async Task GetByIdWoksCorrectlyWhenCityExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
            };

            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
            var cities = await service.GetByIdAsync<CityServiceModel>(1);

            Assert.NotNull(cities);
        }

        [Fact]
        public async Task GetByIdShouldReturnWhenCityDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
            };

            dbContext.Add(city);
            await dbContext.SaveChangesAsync();
            var cities = await service.GetByIdAsync<CityServiceModel>(3);

            Assert.Null(cities);
        }

        [Fact]
        public async Task DeleteByCountryIdWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
                CountryId = 1,
            };

            var city2 = new City()
            {
                Id = 2,
                CountryId = 1,
            };

            var city3 = new City()
            {
                Id = 3,
                CountryId = 2,
            };

            dbContext.Add(city);
            dbContext.Add(city2);
            dbContext.Add(city3);
            await dbContext.SaveChangesAsync();
            await service.DeleteByCountryIdAsync(1);

            var citiesInDb = dbContext.Cities.ToList();

            Assert.Single(citiesInDb);
        }

        [Fact]
        public async Task GetAllAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var addressesService = new Mock<IAddressesService>();

            var repository = new EfDeletableEntityRepository<City>(dbContext);

            var service = new CitiesService(repository, addressesService.Object);
            var city = new City()
            {
                Id = 1,
                CountryId = 1,
            };

            var city2 = new City()
            {
                Id = 2,
                CountryId = 1,
            };

            var city3 = new City()
            {
                Id = 3,
                CountryId = 2,
            };

            dbContext.Add(city);
            dbContext.Add(city2);
            dbContext.Add(city3);
            await dbContext.SaveChangesAsync();

            var result = await service.GetAllAsync<CityServiceModel>();

            Assert.Equal(3, result.Count());
        }
    }
}
