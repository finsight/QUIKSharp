// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp
{
    // http://blogs.msdn.com/b/pfxteam/archive/2012/02/11/10266920.aspx
    internal class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

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
                    Interlocked.CompareExchange(ref m_tcs, new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously), tcs) == tcs)
                    return;
            }
        }
    }
}