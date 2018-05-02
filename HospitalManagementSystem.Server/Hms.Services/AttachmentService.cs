namespace Hms.Services
{
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;
    using Hms.Services.Interface;

    public class AttachmentService : IAttachmentService
    {
        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            this.AttachmentRepository = attachmentRepository;
        }

        public IAttachmentRepository AttachmentRepository { get; }

        public Task<Attachment> GetAttachmentAsync(string login, int attachmentId)
        {
            return this.AttachmentRepository.GetAttachmentAsync(login, attachmentId);
        }

        public Task<AttachmentInfo> GetAttachmentInfoAsync(string login, int attachmentId)
        {
            return this.AttachmentRepository.GetAttachmentInfoAsync(login, attachmentId);
        }
    }
}
