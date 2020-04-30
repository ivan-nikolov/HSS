namespace Hss.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data;
    using Hss.Data.Models;
    using Hss.Data.Repositories;
    using Hss.Services.Data.Addresses;
    using Hss.Services.Models.Addresses;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class AddressesServiceTests : TestsBase
    {
        [Fact]
        public async Task CreateAsyncWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var service = new AddressesService(repository);

            await service.CreateAsync(new AddressServiceModel());
            var addressesInDbCount = repository.All().ToList().Count();

            Assert.Equal(1, addressesInDbCount);
        }

        [Fact]
        public async Task CheckIfCheckIfAddressIsValidForUserReturnsTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
                UserId = Guid.NewGuid().ToString(),
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            var result = service.CheckIfAddressIsValidForUser(1, address.UserId);

            Assert.True(result);
        }

        [Fact]
        public async Task CheckIfCheckIfAddressIsValidForUserReturnsFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
                UserId = Guid.NewGuid().ToString(),
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            var result = service.CheckIfAddressIsValidForUser(1, Guid.NewGuid().ToString());

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            await service.DeleteAsync(1);
            var addressesInDbCount = repository.All().ToList().Count();

            Assert.Equal(0, addressesInDbCount);
        }

        [Fact]
        public async Task DeleteReturnsIfAddressDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            await service.DeleteAsync(2);
            var addressesInDbCount = repository.All().ToList().Count();

            Assert.Equal(1, addressesInDbCount);
        }

        [Fact]
        public async Task DeleteByCityIdWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
                CityId = 1,
            };

            var addressTwo = new Address()
            {
                Id = 2,
                CityId = 1,
            };

            dbContext.Add(address);
            dbContext.Add(addressTwo);
            await dbContext.SaveChangesAsync();

            var service = new AddressesService(repository);
            await service.DeleteByCityIdAsync(1);
            var addressesInDbCount = repository.All().ToList().Count();

            Assert.Equal(0, addressesInDbCount);
        }

        [Fact]
        public async Task EditWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
                CityId = 1,
                Appartment = "1",
                BuildingNumber = "1",
                Neighborhood = "1",
                PostCode = "1",
                StreetName = "1",
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);

            var editedAddress = new AddressServiceModel()
            {
                Id = 1,
                CityId = 2,
                Appartment = "2",
                BuildingNumber = "2",
                Neighborhood = "2",
                PostCode = "2",
                StreetName = "2",
            };

            await service.EditAsync(editedAddress);
            var addressInDb = repository.All().FirstOrDefault();

            Assert.Equal(editedAddress.CityId, addressInDb.CityId);
            Assert.Equal(editedAddress.Appartment, addressInDb.Appartment);
            Assert.Equal(editedAddress.BuildingNumber, addressInDb.BuildingNumber);
            Assert.Equal(editedAddress.Neighborhood, addressInDb.Neighborhood);
            Assert.Equal(editedAddress.PostCode, addressInDb.PostCode);
            Assert.Equal(editedAddress.StreetName, addressInDb.StreetName);
        }

        [Fact]
        public async Task EditAsyncReturnsIfAddressDoesntExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var address = new Address()
            {
                Id = 1,
            };

            var editedAddress = new AddressServiceModel()
            {
                Id = 2,
            };

            dbContext.Add(address);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            await service.EditAsync(editedAddress);
            var addressesInDbCount = repository.All().ToList().Count();

            Assert.Equal(1, addressesInDbCount);
        }

        [Fact]
        public async Task GetUserAddressesWorksCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var repository = new EfDeletableEntityRepository<Address>(dbContext);

            var userId = Guid.NewGuid().ToString();

            var address = new Address()
            {
                Id = 1,
                UserId = userId,
            };

            var addressTwo = new Address()
            {
                Id = 2,
                UserId = userId,
            };

            var addressThree = new Address()
            {
                Id = 3,
                UserId = Guid.NewGuid().ToString(),
            };

            dbContext.Add(address);
            dbContext.Add(addressTwo);
            dbContext.Add(addressThree);
            await dbContext.SaveChangesAsync();
            var service = new AddressesService(repository);
            var addresses = service.GetUserAddresses<AddressServiceModel>(userId);

            Assert.Equal(2, addresses.Count());
        }
    }
}
