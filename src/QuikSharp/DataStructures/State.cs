// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

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