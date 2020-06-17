// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp
{
    /// <summary>
    ///
    /// </summary>
    public enum InfoParams
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Версия программы
        /// </summary>
        VERSION,

        /// <summary>
        /// Дата торгов
        /// </summary>
        TRADEDATE,

        /// <summary>
        /// Время сервера
        /// </summary>
        SERVERTIME,

        /// <summary>
        /// Время последней записи
        /// </summary>
        LASTRECORDTIME,

        /// <summary>
        /// Число записей
        /// </summary>
        NUMRECORDS,

        /// <summary>
        /// Последняя запись
        /// </summary>
        LASTRECORD,

        /// <summary>
        /// Отставшая запись
        /// </summary>
        LATERECORD,

        /// <summary>
        /// Соединение
        /// </summary>
        CONNECTION,

        /// <summary>
        /// IP-адрес сервера
        /// </summary>
        IPADDRESS,

        /// <summary>
        /// Порт сервера
        /// </summary>
        IPPORT,

        /// <summary>
        /// Описание соединения
        /// </summary>
        IPCOMMENT,

        /// <summary>
        /// Описание сервера
        /// </summary>
        SERVER,

        /// <summary>
        /// Идентификатор сессии
        /// </summary>
        SESSIONID,

        /// <summary>
        /// Пользователь
        /// </summary>
        USER,

        /// <summary>
        /// ID пользователя
        /// </summary>
        USERID,

        /// <summary>
        /// Организация
        /// </summary>
        ORG,

        /// <summary>
        /// Занято памяти
        /// </summary>
        MEMORY,

        /// <summary>
        /// Текущее время
        /// </summary>
        LOCALTIME,

        /// <summary>
        /// Время на связи
        /// </summary>
        CONNECTIONTIME,

        /// <summary>
        /// Передано сообщений
        /// </summary>
        MESSAGESSENT,

        /// <summary>
        /// Передано всего байт
        /// </summary>
        ALLSENT,

        /// <summary>
        /// Передано полезных байт
        /// </summary>
        BYTESSENT,

        /// <summary>
        /// Передано за секунду
        /// </summary>
        BYTESPERSECSENT,

        /// <summary>
        /// Принято сообщений
        /// </summary>
        MESSAGESRECV,

        /// <summary>
        /// Принято полезных байт
        /// </summary>
        BYTESRECV,

        /// <summary>
        /// Принято всего байт
        /// </summary>
        ALLRECV,

        /// <summary>
        /// Принято за секунду
        /// </summary>
        BYTESPERSECRECV,

        /// <summary>
        /// Средняя скорость передачи
        /// </summary>
        AVGSENT,

        /// <summary>
        /// Средняя скорость приема
        /// </summary>
        AVGRECV,

        /// <summary>
        /// Время последней проверки связи
        /// </summary>
        LASTPINGTIME,

        /// <summary>
        /// Задержка данных при обмене с сервером
        /// </summary>
        LASTPINGDURATION,

        /// <summary>
        /// Средняя задержка данных
        /// </summary>
        AVGPINGDURATION,

        /// <summary>
        /// Время максимальной задержки
        /// </summary>
        MAXPINGTIME,

        /// <summary>
        /// Максимальная задержка данных
        /// </summary>
        MAXPINGDURATION

        // ReSharper restore InconsistentNaming
    }
}