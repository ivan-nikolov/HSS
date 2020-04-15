﻿namespace Hss.Services.Data.CIties
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hss.Data.Common.Repositories;
    using Hss.Data.Models;
    using Hss.Services.Data.Addresses;
    using Hss.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CitiesService : ICitiesService
    {
        private readonly IDeletableEntityRepository<City> citiesRepository;
        private readonly IAddressesService addressesService;

        public CitiesService(IDeletableEntityRepository<City> citiesRepository, IAddressesService addressesService)
        {
            this.citiesRepository = citiesRepository;
            this.addressesService = addressesService;
        }

        public async Task<bool> CheckIfCityExists(int id)
            => await this.citiesRepository.All()
            .Where(c => c.Id == id)
            .CountAsync() > 0;

        public async Task CreateAsync(string name, int countryId)
        {
            var city = new City()
            {
                Name = name,
                CountryId = countryId,
            };

            await this.citiesRepository.AddAsync(city);
            await this.citiesRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var city = await this.citiesRepository.GetByIdAsync(id);
            await this.addressesService.DeleteByCityIdAsync(id);
            this.citiesRepository.Delete(city);
            await this.citiesRepository.SaveChangesAsync();
        }

        public async Task DeleteByCountryIdAsync(int countryId)
        {
            var cities = await this.GetByCountryIdAsync<City>(countryId);
            foreach (var city in cities)
            {
                await this.addressesService.DeleteByCityIdAsync(city.Id);
                this.citiesRepository.Delete(city);
            }

            await this.citiesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetByCountryIdAsync<T>(int countryId)
            => await this.citiesRepository
            .All()
            .Where(c => c.CountryId == countryId)
            .To<T>()
            .ToListAsync();

        public async Task<T> GetById<T>(int id)
            => await this.citiesRepository.All()
            .Where(c => c.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();
    }
}