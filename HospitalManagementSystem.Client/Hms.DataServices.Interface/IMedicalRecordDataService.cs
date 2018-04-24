namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IMedicalRecordDataService
    {
        Task<MedicalCardRecord> GetMedicalCardRecordAsync(int recordId);
    }
}
