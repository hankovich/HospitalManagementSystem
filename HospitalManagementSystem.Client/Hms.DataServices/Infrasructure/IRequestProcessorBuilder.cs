namespace Hms.DataServices.Infrasructure
{
    public interface IRequestProcessorBuilder
    {
        RequestProcessor Build(ClientStateModel stateModel);
        RequestProcessorBuilder UseEncryption(bool needEncryption);
    }
}