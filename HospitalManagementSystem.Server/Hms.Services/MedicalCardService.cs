namespace Hms.Services
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class MedicalCardService : IMedicalCardService
    {
        public MedicalCardService(IMedicalCardRepository medicalCardRepository)
        {
            if (medicalCardRepository == null)
            {
                throw new ArgumentNullException(nameof(medicalCardRepository));
            }

            this.MedicalCardRepository = medicalCardRepository;
        }

        public IMedicalCardRepository MedicalCardRepository { get; set; }

        public async Task<MedicalCard> GetMedicalCardPagesAsync(string login, int pageIndex, int pageSize = 20, string filter = "")
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            return await this.MedicalCardRepository.GetMedicalCardPagesAsync(login, pageIndex, pageSize, filter);
        }
    }
}