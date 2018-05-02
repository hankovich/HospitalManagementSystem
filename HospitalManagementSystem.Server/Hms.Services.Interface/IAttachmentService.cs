namespace Hms.Services.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAttachmentService
    {
        Task<Attachment> GetAttachmentAsync(string login, int attachmentId);

        Task<AttachmentInfo> GetAttachmentInfoAsync(string login, int attachmentId);
    }
}
