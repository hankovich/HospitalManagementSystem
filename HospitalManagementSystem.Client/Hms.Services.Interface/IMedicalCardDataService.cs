namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalCardDataService
    {
        Task<MedicalCard> GetMedicalCardAsync(int pageIndex, int pageSize = 20);
    }
}
