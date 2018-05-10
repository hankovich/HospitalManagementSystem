namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IPolyclinicRepository
    {
        Task<Polyclinic> GetPolyclinicAsync(int id);

        Task<int> InsertOrUpdatePolyclinicAsync(Polyclinic polyclinic);
    }
}
