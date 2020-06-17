// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблица с параметрами для функции getParamEx
    /// </summary>
    public class ParamTable : IWithLuaTimeStamp
    {
        /// <summary>
        /// Тип данных параметра, используемый в Таблице текущих значений параметров. Возможные значения:
        /// «1» - DOUBLE ,
        /// «2» - LONG,
        /// «3» - CHAR,
        /// «4» - перечислимый тип,
        /// «5» - время,
        /// «6» - дата
        /// </summary>
        [JsonProperty("param_type")]
        public string ParamType { get; set; }

        /// <summary>
        /// Значение параметра. Для param_type = 3 значение параметра равно «0», в остальных случаях – числовое представление.
        /// Для перечислимых типов значение равно порядковому значению перечисления
        /// </summary>
        [JsonProperty("param_value")]
        public string ParamValue { get; set; }

        /// <summary>
        /// Строковое значение параметра, аналогичное его представлению в таблице.
        /// В строковом представлении учитываются разделители разрядов, разделители целой и дробной части.
        /// Для перечислимых типов выводятся соответствующие им строковые значения
        /// </summary>
        [JsonProperty("param_image")]
        public string ParamImage { get; set; }

        public long LuaTimeStamp { get; set; }
    }
}