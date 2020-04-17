namespace Hss.Services.Data.Addresses
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Addresses;
    using Microsoft.EntityFrameworkCore;

    public class AddressesService : IAddressesService
    {
        private readonly IDeletableEntityRepository<Address> addressesRepository;

        public AddressesService(IDeletableEntityRepository<Address> addressesRepository)
        {
            this.addressesRepository = addressesRepository;
        }

        public async Task CreateAsync(AddressServiceModel input)
        {
            var address = input.To<Address>();
            await this.addressesRepository.AddAsync(address);
            await this.addressesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await this.addressesRepository.GetByIdAsync(id);
            if (address == null)
            {
                return;
            }

            this.addressesRepository.Delete(address);
            await this.addressesRepository.SaveChangesAsync();
        }

        public async Task DeleteByCityIdAsync(int cityId)
        {
            var addresses = await this.addressesRepository.All()
                .Where(a => a.CityId == cityId)
                .ToListAsync();

            foreach (var address in addresses)
            {
                this.addressesRepository.Delete(address);
            }

            await this.addressesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(AddressServiceModel input)
        {
            var address = await this.addressesRepository.GetByIdAsync(input.Id);
            if (address == null)
            {
                return;
            }

            address.Appartment = input.Appartment;
            address.BuildingNumber = input.BuildingNumber;
            address.CityId = input.CityId;
            address.Neighborhood = input.Neighborhood;
            address.StreetName = input.StreetName;
            address.PostCode = input.PostCode;

            this.addressesRepository.Update(address);
            await this.addressesRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetUserAddresses<T>(string userId)
            => this.addressesRepository
                         .All()
                         .Where(a => a.UserId == userId)
                         .To<T>()
                         .ToList();
    }
}
