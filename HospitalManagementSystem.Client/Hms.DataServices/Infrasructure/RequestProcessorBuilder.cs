namespace Hms.DataServices.Infrasructure
{
    using Hms.Common.Interface;
    using Hms.DataServices.Interface.Infrastructure;

    public class RequestProcessorBuilder : IRequestProcessorBuilder
    {
        public RequestProcessorBuilder(ISymmetricCryptoProvider symmetricCryptoProvider, IHttpContentProcessor httpContentService)
        {
            this.SymmetricCryptoProvider = symmetricCryptoProvider;
            this.HttpContentProcessor = httpContentService;
        }

        private ISymmetricCryptoProvider SymmetricCryptoProvider { get; }

        private IHttpContentProcessor HttpContentProcessor { get; }

        private bool NeedEncryption { get; set; }

        public RequestProcessorBuilder UseEncryption(bool needEncryption)
        {
            this.NeedEncryption = needEncryption;
            return this;
        }

        public RequestProcessor Build(ClientStateModel stateModel)
        {
            return new RequestProcessor(this.SymmetricCryptoProvider, this.HttpContentProcessor, stateModel)
            {
                NeedEncryption = this.NeedEncryption
            };
        }
    }
}