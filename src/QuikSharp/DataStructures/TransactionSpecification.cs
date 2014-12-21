using System;
using Newtonsoft.Json;

namespace QuikSharp {


    /// <summary>
    /// Вид транзакции, имеющий одно из следующих значений: 
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionAction>))]
    public enum TransactionAction {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// новая заявка,
        /// </summary>
        NEW_ORDER = 1,
        /// <summary>
        /// новая заявка на внебиржевую сделку,
        /// </summary>
        NEW_NEG_DEAL,
        /// <summary>
        /// новая заявка на сделку РЕПО,
        /// </summary>
        NEW_REPO_NEG_DEAL,
        /// <summary>
        /// новая заявка на сделку модифицированного РЕПО (РЕПО-М),
        /// </summary>
        NEW_EXT_REPO_NEG_DEAL,
        /// <summary>
        /// новая стоп-заявка,
        /// </summary>
        NEW_STOP_ORDER,
        /// <summary>
        /// снять заявку,
        /// </summary>
        KILL_ORDER,
        /// <summary>
        /// снять заявку на внебиржевую сделку или заявку на сделку РЕПО,
        /// </summary>
        KILL_NEG_DEAL,
        /// <summary>
        /// снять стоп-заявку,
        /// </summary>
        KILL_STOP_ORDER,
        /// <summary>
        /// снять все заявки из торговой системы,
        /// </summary>
        KILL_ALL_ORDERS,
        /// <summary>
        /// снять все стоп-заявки,
        /// </summary>
        KILL_ALL_STOP_ORDERS,
        /// <summary>
        /// снять все заявки на внебиржевые сделки и заявки на сделки РЕПО,
        /// </summary>
        KILL_ALL_NEG_DEALS,
        /// <summary>
        /// снять все заявки на рынке FORTS,
        /// </summary>
        KILL_ALL_FUTURES_ORDERS,
        /// <summary>
        /// удалить лимит открытых позиций на спот-рынке RTS Standard,
        /// </summary>
        KILL_RTS_T4_LONG_LIMIT,
        /// <summary>
        /// удалить лимит открытых позиций клиента по спот-активу на рынке RTS Standard,
        /// </summary>
        KILL_RTS_T4_SHORT_LIMIT,
        /// <summary>
        /// переставить заявки на рынке FORTS,
        /// </summary>
        MOVE_ORDERS,
        /// <summary>
        /// новая безадресная заявка,
        /// </summary>
        NEW_QUOTE,
        /// <summary>
        /// снять безадресную заявку,
        /// </summary>
        KILL_QUOTE,
        /// <summary>
        /// новая  заявка-отчет о подтверждении транзакций в режимах РПС и РЕПО,
        /// </summary>
        NEW_REPORT,
        /// <summary>
        /// новое ограничение по фьючерсному счету
        /// </summary>
        SET_FUT_LIMIT,
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Тип заявки, необязательный параметр.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionType>))]
    public enum TransactionType {
        /// <summary>
        /// «L» - лимитированная (по умолчанию), 
        /// </summary>
        L = 0,
        /// <summary>
        /// «M» – рыночная
        /// </summary>
        M
    }

    /// <summary>
    /// YES or NO
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<YesOrNo>))]
    public enum YesOrNo {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        NO = 1,
        /// <summary>
        /// 
        /// </summary>
        YES
        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// YES or NO with NO default
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<YesOrNoDefault>))]
    public enum YesOrNoDefault {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        NO = 0,
        /// <summary>
        /// 
        /// </summary>
        YES
        // ReSharper restore InconsistentNaming
    }


    /// <summary>
    /// Тип заявки, необязательный параметр.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionOperation>))]
    public enum TransactionOperation {
        /// <summary>
        /// «B» – купить
        /// </summary>
        B = 1,
        /// <summary>
        /// «S» – продать
        /// </summary>
        S = 2
    }

    /// <summary>
    /// Условие исполнения заявки, необязательный параметр. Возможные значения:
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionOperation>))]
    public enum ExecutionCondition {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// «PUT_IN_QUEUE» – поставить в очередь (по умолчанию),
        /// </summary>

        PUT_IN_QUEUE = 0,

        /// <summary>
        /// «FILL_OR_KILL» – немедленно или отклонить,
        /// </summary>
        FILL_OR_KILL = 1,

        /// <summary>
        /// «KILL_BALANCE» – снять остаток
        /// </summary>
        KILL_BALANCE = 2

        // ReSharper restore InconsistentNaming
    }


    /// <summary>
    /// Тип стоп-заявки. Возможные значения:
    /// Если параметр пропущен, то считается, что заявка имеет тип «стоп-лимит»
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<StopOrderKind>))]
    public enum StopOrderKind {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// стоп-лимит,
        /// Если параметр пропущен, то считается, что заявка имеет тип «стоп-лимит»
        /// </summary>
        SIMPLE_STOP_ORDER = 0,
        /// <summary>
        /// с условием по другой бумаге,
        /// </summary>
        CONDITION_PRICE_BY_OTHER_SEC,
        /// <summary>
        /// со связанной заявкой,
        /// </summary>
        WITH_LINKED_LIMIT_ORDER,
        /// <summary>
        /// тэйк-профит,
        /// </summary>
        TAKE_PROFIT_STOP_ORDER,
        /// <summary>
        /// тэйк-профит и стоп-лимит,
        /// </summary>
        TAKE_PROFIT_AND_STOP_LIMIT_ORDER,
        /// <summary>
        /// стоп-лимит по исполнению заявки,
        /// </summary>
        ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER,
        /// <summary>
        /// тэйк-профит по исполнению заявки,
        /// </summary>
        ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER,
        /// <summary>
        /// тэйк-профит и стоп-лимит по исполнению заявки.
        /// </summary>
        ACTIVATED_BY_ORDER_TAKE_PROFIT_AND_STOP_LIMIT_ORDER,


        // ReSharper restore InconsistentNaming
    }


    /// <summary>
    /// Лицо, от имени которого и за чей счет регистрируется сделка 
    /// (параметр внебиржевой сделки). Возможные значения:
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<ForAccount>))]
    public enum ForAccount {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// от своего имени, за свой счет,
        /// </summary>
        OWNOWN = 1,
        /// <summary>
        /// от своего имени, за счет клиента,
        /// </summary>
        OWNCLI,
        /// <summary>
        /// от своего имени, за счет доверительного управления,
        /// </summary>
        OWNDUP,
        /// <summary>
        /// от имени клиента, за счет клиента
        /// </summary>
        CLICLI,

        // ReSharper restore InconsistentNaming
    }


    /// <summary>
    /// Единицы измерения отступа. Используется при «STOP_ORDER_KIND» = 
    /// «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<OffsetUnits>))]
    public enum OffsetUnits {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// «PERCENTS» – в процентах (шаг изменения – одна сотая процента),
        /// </summary>
        PERCENTS = 1,
        /// <summary>
        /// «PRICE_UNITS» – в параметрах цены (шаг изменения равен шагу цены по данному инструменту).
        /// </summary>
        PRICE_UNITS = 2
        // ReSharper restore InconsistentNaming
    }



    /// <summary>
    /// Формат .tri-файла с параметрами транзакций
    /// Адоптированный под QLua
    /// </summary>
    public class TransactionSpecification {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Код класса, по которому выполняется транзакция, например TQBR. Обязательный параметр
        /// </summary>
        public string CLASSCODE { get; set; }
        /// <summary>
        /// Код инструмента, по которому выполняется транзакция, например SBER
        /// </summary>
        public string SECCODE { get; set; }
        /// <summary>
        /// Вид транзакции, имеющий одно из следующих значений:
        /// </summary>
        public TransactionAction ACTION { get; set; }
        /// <summary>
        /// Идентификатор участника торгов (код фирмы)
        /// </summary>
        public string FIRM_ID { get; set; }
        /// <summary>
        /// Номер счета Трейдера. Параметр обязателен при «ACTION» = «KILL_ALL_FUTURES_ORDERS». Параметр чувствителен к верхнему/нижнему регистру символов.
        /// </summary>
        public string ACCOUNT { get; set; }
        /// <summary>
        /// 20-ти символьное составное поле, может содержать код клиента и текстовый комментарий с тем же разделителем, что и при вводе заявки вручную. Параметр используется только для групповых транзакций. Необязательный параметр
        /// </summary>
        public string CLIENT_CODE { get; set; }
        /// <summary>
        /// Тип заявки, необязательный параметр. Значения: «L» – лимитированная (по умолчанию), «M» – рыночная
        /// </summary>
        public TransactionType TYPE { get; set; }
        /// <summary>
        /// Признак того, является ли заявка заявкой Маркет-Мейкера. Возможные значения: «YES» или «NO». Значение по умолчанию (если параметр отсутствует): «NO»
        /// </summary>
        public YesOrNoDefault MARKET_MAKER_ORDER { get; set; }
        /// <summary>
        /// Направление заявки, обязательный параметр. Значения: «S» – продать, «B» – купить
        /// </summary>
        public TransactionOperation OPERATION { get; set; }
        /// <summary>
        /// Условие исполнения заявки, необязательный параметр. Возможные значения:
        /// </summary>
        public ExecutionCondition EXECUTION_CONDITION { get; set; }
        /// <summary>
        /// Количество лотов в заявке, обязательный параметр
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int>))]
        public int QUANTITY { get; set; }
        /// <summary>
        /// Цена заявки, за единицу инструмента. Обязательный параметр. При выставлении рыночной заявки (TYPE=M) на Срочном рынке FORTS необходимо указывать значение цены – укажите наихудшую (минимально или максимально возможную – в зависимости от направленности), заявка все равно будет исполнена по рыночной цене. Для других рынков при выставлении рыночной заявки укажите price= 0.
        /// </summary>
        public double PRICE { get; set; }
        /// <summary>
        /// Объем сделки РЕПО-М в рублях
        /// </summary>
        public double? REPOVALUE { get; set; }
        /// <summary>
        /// Начальное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        public double? START_DISCOUNT { get; set; }
        /// <summary>
        /// Нижнее предельное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        public double? LOWER_DISCOUNT { get; set; }
        /// <summary>
        /// Верхнее предельное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        public double? UPPER_DISCOUNT { get; set; }
        /// <summary>
        /// Стоп-цена, за единицу инструмента. Используется только при «ACTION» = «NEW_STOP_ORDER»
        /// </summary>
        public double? STOPPRICE { get; set; }
        /// <summary>
        /// Тип стоп-заявки. Возможные значения:
        /// </summary>
        public StopOrderKind STOP_ORDER_KIND { get; set; }
        /// <summary>
        /// Класс инструмента условия. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC».
        /// </summary>
        public string STOPPRICE_CLASSCODE { get; set; }
        /// <summary>
        /// Код инструмента условия. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC»
        /// </summary>
        public string STOPPRICE_SECCODE { get; set; }
        /// <summary>
        /// Направление предельного изменения стоп-цены. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC». Возможные значения:  «<=» или «>= »
        /// </summary>
        public string STOPPRICE_CONDITION { get; set; }
        /// <summary>
        /// Цена связанной лимитированной заявки. Используется только при «STOP_ORDER_KIND» = «WITH_LINKED_LIMIT_ORDER»
        /// </summary>
        public double? LINKED_ORDER_PRICE { get; set; }
        /// <summary>
        /// Срок действия стоп-заявки. Возможные значения: «GTC» – до отмены, «TODAY» - до окончания текущей торговой сессии, Дата в формате «ГГММДД».
        /// </summary>
        public string EXPIRY_DATE { get; set; }
        /// <summary>
        /// Цена условия «стоп-лимит» для заявки типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        public double? STOPPRICE2 { get; set; }
        /// <summary>
        /// Признак исполнения заявки по рыночной цене при наступлении условия «стоп-лимит». Значения «YES» или «NO». Параметр заявок типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 MARKET_STOP_LIMIT	{get; set;}
        /// <summary>
        /// Признак исполнения заявки по рыночной цене при наступлении условия «тэйк-профит». Значения «YES» или «NO». Параметр заявок типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 MARKET_TAKE_PROFIT	{get; set;}
        /// <summary>
        /// Признак действия заявки типа «Тэйк-профит и стоп-лимит» в течение определенного интервала времени. Значения «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 IS_ACTIVE_IN_TIME	{get; set;}
        /// <summary>
        /// Время начала действия заявки типа «Тэйк-профит и стоп-лимит» в формате «ЧЧММСС»
        /// </summary>
        public DateTime? ACTIVE_FROM_TIME { get; set; }
        /// <summary>
        /// Время окончания действия заявки типа «Тэйк-профит и стоп-лимит» в формате «ЧЧММСС»
        /// </summary>
        public DateTime? ACTIVE_TO_TIME { get; set; }
        /// <summary>
        /// Код организации – партнера по внебиржевой сделке.Применяется при «ACTION» = «NEW_NEG_DEAL», «ACTION» = «NEW_REPO_NEG_DEAL» или «ACTION» = «NEW_EXT_REPO_NEG_DEAL»
        /// </summary>
        public string PARTNER { get; set; }
        /// <summary>
        /// Номер заявки, снимаемой из торговой системы. Применяется при «ACTION» = «KILL_ORDER» или «ACTION» = «KILL_NEG_DEAL» или «ACTION» = «KILL_QUOTE»
        /// </summary>
        public string ORDER_KEY { get; set; }
        /// <summary>
        /// Номер стоп-заявки, снимаемой из торговой системы. Применяется только при «ACTION» = «KILL_STOP_ORDER»
        /// </summary>
        public string STOP_ORDER_KEY { get; set; }
        /// <summary>
        /// Уникальный идентификационный номер заявки, значение от 1 до 2 294 967 294
        /// </summary>
        public long? TRANS_ID { get; set; }
        /// <summary>
        /// Код расчетов при исполнении внебиржевых заявок
        /// </summary>
        public string SETTLE_CODE { get; set; }
        /// <summary>
        /// Цена второй части РЕПО
        /// </summary>
        public double? PRICE2 { get; set; }
        /// <summary>
        /// Срок РЕПО. Параметр сделок РЕПО-М
        /// </summary>
        public string REPOTERM { get; set; }
        /// <summary>
        /// Ставка РЕПО, в процентах
        /// </summary>
        public string REPORATE { get; set; }
        /// <summary>
        /// Признак блокировки бумаг на время операции РЕПО («YES», «NO»)
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 BLOCK_SECURITIES	{get; set;}
        /// <summary>
        /// Ставка фиксированного возмещения, выплачиваемого в случае неисполнения второй части РЕПО, в процентах
        /// </summary>
        public double? REFUNDRATE { get; set; }
        /// <summary>
        /// Текстовый комментарий, указанный в заявке. Используется при снятии группы заявок
        /// </summary>
        public string COMMENT { get; set; }
        /// <summary>
        /// Признак крупной сделки (YES/NO). Параметр внебиржевой сделки
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 LARGE_TRADE	{get; set;}
        /// <summary>
        /// Код валюты расчетов по внебиржевой сделки, например «SUR» – рубли РФ, «USD» – доллары США. Параметр внебиржевой сделки
        /// </summary>
        public string CURR_CODE { get; set; }
        /// <summary>
        /// Лицо, от имени которого и за чей счет регистрируется сделка (параметр внебиржевой сделки). Возможные значения:
        /// </summary>
        public ForAccount FOR_ACCOUNT { get; set; }
        /// <summary>
        /// Дата исполнения внебиржевой сделки
        /// </summary>
        public string SETTLE_DATE { get; set; }
        /// <summary>
        /// Признак снятия стоп-заявки при частичном исполнении связанной лимитированной заявки. Используется только при «STOP_ORDER_KIND» = «WITH_LINKED_LIMIT_ORDER». Возможные значения: «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?
        public 	YesOrNo	 KILL_IF_LINKED_ORDER_PARTLY_FILLED	{get; set;}
        /// <summary>
        /// Величина отступа от максимума (минимума) цены последней сделки. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public double? OFFSET { get; set; }
        /// <summary>
        /// Единицы измерения отступа. Возможные значения:
        /// </summary>
        public OffsetUnits OFFSET_UNITS { get; set; }
        /// <summary>
        /// Величина защитного спрэда. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public double? SPREAD { get; set; }
        /// <summary>
        /// Единицы измерения защитного спрэда. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public OffsetUnits SPREAD_UNITS { get; set; }
        /// <summary>
        /// Регистрационный номер заявки-условия. Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public string BASE_ORDER_KEY { get; set; }
        /// <summary>
        /// Признак использования в качестве объема заявки «по исполнению» исполненного количества бумаг заявки-условия. Возможные значения: «YES» или «NO». Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 USE_BASE_ORDER_BALANCE	{get; set;}
        /// <summary>
        /// Признак активации заявки «по исполнению» при частичном исполнении заявки-условия. Возможные значения: «YES» или «NO». Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 ACTIVATE_IF_BASE_ORDER_PARTLY_FILLED	{get; set;}
        /// <summary>
        /// Идентификатор базового контракта для фьючерсов или опционов. Обязательный параметр снятия заявок на рынке FORTS
        /// </summary>
        public string BASE_CONTRACT { get; set; }
        /// <summary>
        ///  Режим перестановки заявок на рынке FORTS. Параметр операции «ACTION» = «MOVE_ORDERS» Возможные значения: «0» – оставить количество в заявках без изменения, «1» – изменить количество в заявках на новые, «2» – при несовпадении новых количеств с текущим хотя бы в одной заявке, обе заявки снимаются
        /// </summary>
        public string MODE { get; set; }
        /// <summary>
        /// Номер первой заявки
        /// </summary>
        public long? FIRST_ORDER_NUMBER { get; set; }
        /// <summary>
        /// Количество в первой заявке
        /// </summary>
        public int? FIRST_ORDER_NEW_QUANTITY { get; set; }
        /// <summary>
        /// Цена в первой заявке
        /// </summary>
        public double? FIRST_ORDER_NEW_PRICE { get; set; }
        /// <summary>
        /// Номер второй заявки
        /// </summary>
        public long? SECOND_ORDER_NUMBER { get; set; }
        /// <summary>
        /// Количество во второй заявке
        /// </summary>
        public int? SECOND_ORDER_NEW_QUANTITY { get; set; }
        /// <summary>
        /// Цена во второй заявке
        /// </summary>
        public double? SECOND_ORDER_NEW_PRICE { get; set; }
        /// <summary>
        /// Признак снятия активных заявок по данному инструменту. Используется только при «ACTION» = «NEW_QUOTE». Возможные значения: «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 KILL_ACTIVE_ORDERS	{get; set;}
        /// <summary>
        /// Направление операции в сделке, подтверждаемой отчетом
        /// </summary>
        public string NEG_TRADE_OPERATION { get; set; }
        /// <summary>
        /// Номер подтверждаемой отчетом сделки для исполнения
        /// </summary>
        public long? NEG_TRADE_NUMBER { get; set; }
        /// <summary>
        /// Лимит открытых позиций, при «Тип лимита» = «Ден.средства» или «Всего»
        /// </summary>
        public string VOLUMEMN { get; set; }
        /// <summary>
        /// Лимит открытых позиций, при «Тип лимита» = «Залоговые ден.средства»
        /// </summary>
        public string VOLUMEPL { get; set; }
        /// <summary>
        /// Коэффициент ликвидности
        /// </summary>
        public string KFL { get; set; }
        /// <summary>
        /// Коэффициент клиентского гарантийного обеспечения
        /// </summary>
        public string KGO { get; set; }
        /// <summary>
        /// Параметр, который определяет, будет ли загружаться величина КГО при загрузке лимитов из файла: при USE_KGO=Y – величина КГО загружает. при USE_KGO=N – величина КГО не загружается. При установке лимита на Срочном рынке Московской Биржи с принудительным понижением (см. Создание лимита) требуется указать USE_KGO= Y
        /// </summary>
        public string USE_KGO { get; set; }
        /// <summary>
        /// Признак проверки попадания цены заявки в диапазон допустимых цен. Параметр Срочного рынка FORTS. Необязательный параметр транзакций установки новых заявок по классам «Опционы ФОРТС» и «РПС: Опционы ФОРТС». Возможные значения: «YES» - выполнять проверку, «NO» - не выполнять
        /// </summary>
        // TODO (?) Is No default here?	
        public 	YesOrNo	 CHECK_LIMITS	{get; set;}
        /// <summary>
        /// Ссылка, которая связывает две сделки РЕПО или РПС. Сделка может быть заключена только между контрагентами, указавшими одинаковое значение этого параметра в своих заявках. Параметр представляет собой набор произвольный набор количеством до 10 символов (допускаются цифры и буквы). Необязательный параметр
        /// </summary>
        public string MATCHREF { get; set; }
        /// <summary>
        /// Режим корректировки ограничения по фьючерсным счетам. Возможные значения: «Y» - включен, установкой лимита изменяется действующее значение, «N» - выключен (по умолчанию), установкой лимита задается новое значение
        /// </summary>
        public string CORRECTION { get; set; }


        // ReSharper restore InconsistentNaming
    }

}