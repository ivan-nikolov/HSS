namespace Hss.Services.Data.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hss.Data.Models.Enums;
    using Hss.Services.Models.Teams;

    public interface ITeamsService
    {
        Task<string> CreateAsync(TeamServiceModel input);

        IEnumerable<T> GetTeamsInLocation<T>(int cityId);

        T GetById<T>(string id);

        Task UpdateAsync(TeamServiceModel input);

        Task<bool> CheckIfTeamExistsAsync(string id);

        Task DeleteAsync(string id);

        bool HasFreeTeams(DateTime currentDate, ServiceFrequency serviceFrequency, int cityId);
    }
}
