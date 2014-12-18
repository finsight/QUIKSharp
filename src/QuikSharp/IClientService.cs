namespace QuikSharp
{
    /// <summary>
    /// Marker service
    /// </summary>
    public interface IClientService
    {
    }


    public interface IDuplexClientService<out T> : IClientService where T : IClientsCallback
    {
        IClientsCallbackProxy<T> ClientsCallbackProxy { get; }
    }

}
