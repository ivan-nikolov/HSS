namespace Hss.Services.Data.Services
{
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;

    public class ServicesService : IServicesService
    {
        private readonly IDeletableEntityRepository<Service> serviceRepository;

        public ServicesService(IDeletableEntityRepository<Service> serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        public async Task CreateAsync(ServiceServiceModel input)
        {
            var service = input.To<Service>();
            await this.serviceRepository.AddAsync(service);
            await this.serviceRepository.SaveChangesAsync();
        }
    }
}
