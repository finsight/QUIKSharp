namespace QuikSharp {
    /// <summary>
    /// A marker interface for all classes that implement Quik functions (grouped by QLUA.chm manual)
    /// .NET interface to QLUA API. Calling the members from .NET is the same as calling 
    /// them from QLUA inside QUIK
    /// </summary>
    public interface IQuikFunctions {
        QuikService QuikService { get; }
    }
    
}