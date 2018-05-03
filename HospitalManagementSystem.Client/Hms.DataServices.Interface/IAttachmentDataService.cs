namespace Hms.DataServices.Interface
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAttachmentDataService
    {
        Task<Attachment> GetAttachmentAsync(int attachmentId, CancellationToken cancellationToken = default(CancellationToken));

        Task<AttachmentInfo> GetAttachmentInfoAsync(int attachmentId);
    }
}