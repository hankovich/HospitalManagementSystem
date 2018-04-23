namespace Hms.UI.Infrastructure.Controls.PagingControl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPageControlContract
    {
        Task<int> GetTotalCountAsync(object filter);

        Task<ICollection<object>> GetRecordsAsync(int startingIndex, int numberOfRecords, object filter);
    }
}
