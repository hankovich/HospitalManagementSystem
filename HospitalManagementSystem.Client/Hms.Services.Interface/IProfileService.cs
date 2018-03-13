﻿namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IProfileService
    {
        Task<Profile> GetProfileAsync(int userId);

        Task<Profile> GetCurrentProfileAsync();

        Task<int> InsertOrUpdateProfileAsync(Profile profile);
    }
}
