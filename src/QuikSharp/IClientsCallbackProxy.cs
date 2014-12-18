using System;

namespace QuikSharp
{
    public interface IClientsCallbackProxy<out T> where T : IClientsCallback
    {
        TRes Call<TRes>(Func<T, TRes> f);
        void Call(Action<T> f);
    }



}
