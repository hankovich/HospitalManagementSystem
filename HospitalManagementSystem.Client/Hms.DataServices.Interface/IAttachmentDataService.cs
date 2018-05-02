namespace Hms.DataServices.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAttachmentDataService
    {
        Task<Attachment> GetAttachmentAsync(int attachmentId);

        Task<AttachmentInfo> GetAttachmentInfoAsync(int attachmentId);
    }
}