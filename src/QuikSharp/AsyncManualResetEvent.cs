using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp
{
    // http://blogs.msdn.com/b/pfxteam/archive/2012/02/11/10266920.aspx
    internal class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>();

        public Task WaitAsync()
        {
            return m_tcs.Task;
        }

        public void Set()
        {
            m_tcs.TrySetResult(true);
        }

        public void Reset()
        {
            while (true)
            {
                var tcs = m_tcs;
                if (!tcs.Task.IsCompleted ||
                    Interlocked.CompareExchange(ref m_tcs, new TaskCompletionSource<bool>(), tcs) == tcs)
                    return;
            }
        }
    }
}