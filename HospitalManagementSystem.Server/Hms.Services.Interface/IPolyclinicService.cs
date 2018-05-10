namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicService
    {
        Task<Polyclinic> GetPolyclinicAsync(int id);

        Task<int> InsertOrUpdatePolyclinicAsync(Polyclinic polyclinic);
    }
}
