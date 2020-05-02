namespace Hss.Services.Data.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Services.Models.Teams;

    public interface ITeamsService
    {
        Task<string> CreateAsync(TeamServiceModel input);

        Task<IEnumerable<T>> GetAll<T>();

        IEnumerable<T> GetTeamsInLocation<T>(int cityId);

        T GetById<T>(string id);

        Task UpdateAsync(TeamServiceModel input);

        Task<bool> CheckIfTeamExistsAsync(string id);

        Task DeleteAsync(string id);

        bool HasFreeTeams(DateTime startDate, DateTime endDate, int cityId, int serviceId);

        IEnumerable<T> GetFreeTeams<T>(DateTime startDate, DateTime endDate, int cityId, int serviceId);
    }
}
