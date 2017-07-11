namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Состояние заявки или стоп-заявки
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Активна
        /// </summary>
        Active,

        /// <summary>
        /// Исполнена
        /// </summary>
        Completed,

        /// <summary>
        /// Снята
        /// </summary>
        Canceled
    }
}