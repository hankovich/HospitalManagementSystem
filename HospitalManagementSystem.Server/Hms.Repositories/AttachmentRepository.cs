namespace Hms.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Repositories.Interface;

    public class AttachmentRepository : IAttachmentRepository
    {
        public AttachmentRepository(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is null or whitespace", nameof(connectionString));
            }

            this.ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public async Task<Attachment> GetAttachmentAsync(string login, int attachmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<AttachmentInfo> GetAttachmentInfoAsync(string login, int attachmentId)
        {
            throw new NotImplementedException();
        }
    }
}
