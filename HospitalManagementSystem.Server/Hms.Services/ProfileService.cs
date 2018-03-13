namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class ProfileService : IProfileService
    {
        public ProfileService(IProfileRepository profileRepository)
        {
            if (profileRepository == null)
            {
                throw new ArgumentNullException(nameof(profileRepository));
            }

            this.ProfileRepository = profileRepository;
        }

        public IProfileRepository ProfileRepository { get; }

        public async Task<Profile> GetProfileAsync(int userId)
        {
            return await this.ProfileRepository.GetProfileAsync(userId);
        }

        public async Task<int> InsertOrUpdateProfileAsync(Profile profile)
        {
            return await this.ProfileRepository.InsertOrUpdateProfileAsync(profile);
        }
    }
}
