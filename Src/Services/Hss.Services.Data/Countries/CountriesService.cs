﻿namespace Hss.Services.Data.Countries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Data.CIties;
    using Hss.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CountriesService : ICountriesService
    {
        private readonly IDeletableEntityRepository<Country> countryRepository;
        private readonly ICitiesService citiesService;

        public CountriesService(IDeletableEntityRepository<Country> countryRepository, ICitiesService citiesService)
        {
            this.countryRepository = countryRepository;
            this.citiesService = citiesService;
        }

        public async Task<bool> CheckIfCountryExists(int id)
            => await this.countryRepository.All()
            .Where(c => c.Id == id)
            .CountAsync() > 0;

        public async Task CreateAsync(string name)
        {
            var country = new Country()
            {
                Name = name,
            };

            await this.countryRepository.AddAsync(country);
            await this.countryRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var country = await this.countryRepository.GetByIdAsync(id);
            if (country == null)
            {
                return;
            }

            await this.citiesService.DeleteByCountryIdAsync(id);
            this.countryRepository.Delete(country);
            await this.countryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.countryRepository.All()
            .To<T>()
            .ToListAsync();

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.countryRepository.All()
            .Where(c => c.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();
    }
}
