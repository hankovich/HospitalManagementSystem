namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicDataService
    {
        Task<Polyclinic> GetPolyclinicAsync(int polyclinicId);
    }
}
