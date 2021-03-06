﻿namespace Hss.Services.Data.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Mapping;
    using Hss.Services.Models.Services;

    using Microsoft.EntityFrameworkCore;

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

        public async Task DeleteAsync(int id)
        {
            var category = await this.serviceRepository
                .GetByIdAsync(id);

            this.serviceRepository.Delete(category);
            await this.serviceRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByCategoryId<T>(int categoryId)
        {
            return this.serviceRepository
                .All()
                .Where(s => s.CategoryId == categoryId)
                .To<T>()
                .ToList();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var category = await this.serviceRepository
                .All().Where(c => c.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return category;
        }

        public int GetCountByCategoryId(int categoryId)
        {
            return this.serviceRepository.All().Count(c => c.CategoryId == categoryId);
        }

        public bool CheckIfServiceExists(int id)
        {
            return this.serviceRepository.All().Count(s => s.Id == id) > 0;
        }

        public async Task UpdateAsync(ServiceServiceModel input)
        {
            var service = await this.serviceRepository
                .GetByIdAsync(input.Id);
            service.Name = input.Name;
            service.CategoryId = input.CategoryId;
            service.Description = input.Description;
            service.Price = input.Price;
            service.ImageUrl = input.ImageUrl;

            this.serviceRepository.Update(service);
            await this.serviceRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllByTeamCity<T>(int cityId)
            => this.serviceRepository.All()
            .Where(s => s.Teams.Any(ts => ts.Team.CityId == cityId))
            .To<T>()
            .ToList();

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await this.serviceRepository
            .All()
            .To<T>()
            .ToListAsync();

        public bool CheckIfServicesExist(IEnumerable<int> serviceIds)
            => this.serviceRepository.All()
            .Select(s => s.Id)
            .ToList()
            .Intersect(serviceIds)
            .Count() == serviceIds.Count();

        public decimal GetServicePrice(int id)
            => this.serviceRepository.All()
            .Where(s => s.Id == id)
            .Select(s => s.Price)
            .FirstOrDefault();

        public int GetServiceDuration(int id)
            => this.serviceRepository.All()
            .Where(s => s.Id == id)
            .Select(s => s.DurationInHours)
            .FirstOrDefault();
    }
}
