namespace Hss.Services.Data.Addresses
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Addresses;

    public interface IAddressesService
    {
        Task CreateAsync(AddressServiceModel input);

        IEnumerable<T> GetUserAddresses<T>(string userId);

        Task EditAsync(AddressServiceModel input);

        Task DeleteAsync(int id);

        Task DeleteByCityIdAsync(int cityId);

        bool CheckIfAddressIsValidForUser(int addressId, string userId);
    }
}
