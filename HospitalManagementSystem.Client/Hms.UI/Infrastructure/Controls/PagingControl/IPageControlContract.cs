namespace Hms.UI.Infrastructure.Controls.PagingControl
{
    using System.Collections;
    using System.Threading.Tasks;

    public interface IPageControlContract
    {
        Task<int> GetTotalCountAsync(object filter);

        Task<IEnumerable> GetRecordsAsync(int startingIndex, int numberOfRecords, object filter);
    }
}
