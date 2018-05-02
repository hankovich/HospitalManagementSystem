namespace Hms.Repositories.Interface
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;

    public interface IAttachmentRepository
    {
        Task<Attachment> GetAttachmentAsync(string login, int attachmentId);

        Task<AttachmentInfo> GetAttachmentInfoAsync(string login, int attachmentId);
    }
}
