namespace Hss.Services.Data.Services
{
    using System.Threading.Tasks;

    using Hss.Services.Models.Services;

    public interface IServicesService
    {
        Task CreateAsync(ServiceServiceModel input);
    }
}
