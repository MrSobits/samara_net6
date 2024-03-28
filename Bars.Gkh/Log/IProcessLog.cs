namespace Bars.Gkh.Log
{
    using B4.Modules.FileStorage;

    /// <summary>
    /// лог процесса
    /// </summary>
    public interface IProcessLog
    {
        /// <summary>
        /// Установить название файла лога
        /// </summary>
        /// <param name="logname"></param>
        void SetLogName(string logname);

        /// <summary>
        /// Добавить сообщение с информацией
        /// </summary>
        void Info(object message, object obj = null);

        /// <summary>
        /// Добавить отладочное сообщение
        /// </summary>
        void Debug(object message, object obj = null);

        /// <summary>
        /// Добавить сообщение об ошибке
        /// </summary>
        void Error(object message, object obj = null);

        /// <summary>
        /// Добавить сообщение о предупреждении
        /// </summary>
        void Warning(object message, object obj = null);

        /// <summary>
        /// Сохранить лог
        /// </summary>
        /// <returns></returns>
        FileInfo Save();
    }
}