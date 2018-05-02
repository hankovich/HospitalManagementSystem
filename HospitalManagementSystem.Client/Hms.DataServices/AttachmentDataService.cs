namespace Hms.DataServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hms.Common.Interface.Domain;
    using Hms.Common.Interface.Exceptions;
    using Hms.DataServices.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class AttachmentDataService : IAttachmentDataService
    {
        public AttachmentDataService(IRequestCoordinator requestCoordinator)
        {
            this.RequestCoordinator = requestCoordinator;
        }

        public IRequestCoordinator RequestCoordinator { get; }

        public async Task<AttachmentInfo> GetAttachmentInfoAsync(int attachmentId)
        {
            var response = await this.RequestCoordinator.SendAsync<AttachmentInfo>(
                HttpMethod.Get,
                $"api/attachments/{attachmentId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }

        public async Task<Attachment> GetAttachmentAsync(int attachmentId)
        {
            var response = await this.RequestCoordinator.SendAsync<Attachment>(
                               HttpMethod.Get,
                               $"api/attachments/info/{attachmentId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new HmsException(response.ReasonPhrase);
            }

            return response.Content;
        }
    }
}
