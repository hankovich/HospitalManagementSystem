namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class PolyclinicService : IPolyclinicService
    {
        public PolyclinicService(IPolyclinicRepository polyclinicRepository)
        {
            if (polyclinicRepository == null)
            {
                throw new ArgumentNullException(nameof(polyclinicRepository));
            }

            this.PolyclinicRepository = polyclinicRepository;
        }

        public IPolyclinicRepository PolyclinicRepository { get; }

        public async Task<Polyclinic> GetPolyclinicAsync(int id)
        {
            return await this.PolyclinicRepository.GetPolyclinicAsync(id);
        }

        public async Task<int> InsertOrUpdatePolyclinicAsync(Polyclinic polyclinic)
        {
            return await this.PolyclinicRepository.InsertOrUpdatePolyclinicAsync(polyclinic);
        }
    }
}
