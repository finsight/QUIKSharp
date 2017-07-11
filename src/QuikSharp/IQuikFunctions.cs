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