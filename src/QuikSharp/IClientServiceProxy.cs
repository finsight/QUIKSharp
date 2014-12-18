using System;

namespace QuikSharp
{
    public interface IClientServiceProxy<out T> where T : IClientService
    {
        TRes Call<TRes>(Func<T, TRes> f);
        bool TryCall<TRes>(Func<T, TRes> f, out TRes result);
    }
}
