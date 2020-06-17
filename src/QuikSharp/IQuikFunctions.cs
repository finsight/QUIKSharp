// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp
{
    /// <summary>
    /// A marker interface for all classes that implement Quik functions (grouped by QLUA.chm manual)
    /// .NET interface to QLUA API. Calling the members from .NET is the same as calling
    /// them from QLUA inside QUIK
    /// </summary>
    public interface IQuikService
    {
        QuikService QuikService { get; }
    }
}