namespace QuikSharp
{
    public interface IEchoService : IClientService
    {
        string Echo(string value);
    }

    public interface IEchoDuplexService : IDuplexClientService<IEchoClientsCallback>
    {
        string Echo(string value);
    }

    public interface IEchoClientsCallback : IClientsCallback
    {
        void Echo(string value);
    }

}
