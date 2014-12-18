namespace QuikSharp.Quik {
    /// <summary>
    /// Implements all Quik callback functions to be processed 
    /// </summary>
    public interface IQuikService : IDuplexClientService<IQuikCallback> {
        /// <summary>
        /// Функция вызывается терминалом QUIK перед вызовом функции main(). В качестве параметра принимает значение полного пути к запускаемому скрипту. 
        /// Примечание: В данной функции пользователь имеет возможность инициализировать все необходимые переменные и библиотеки перед запуском основного потока main()
        /// </summary>
        /// <param name="scriptPath"></param>
        void OnInit(string scriptPath);

        /// <summary>
        /// Функция вызывается терминалом QUIK при остановке скрипта из диалога управления. 
        /// Примечание: Значение параметра «stop_flag» – «1».После окончания выполнения функции таймаут завершения работы скрипта 5 секунд. По истечении этого интервала функция main() завершается принудительно. При этом возможна потеря системных ресурсов.
        /// </summary>
        /// <param name="stopFlag"></param>
        void OnStop(int stopFlag);

    }
}