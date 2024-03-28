namespace Bars.GisIntegration.Tor.Service.LogService
{
    using Bars.B4.Modules.FileStorage;
    using Castle.Windsor;

    public interface ITorLogService
    {
        /// <summary>
        /// Добавляет запись в лог
        /// </summary>
        /// <param name="record">Запись</param>
        /// <param name="torTaskId">Уникальный идентификатор задачи</param>
        void AddLogRecord(string record, long torTaskId);

        /// <summary>
        /// Сохраняет лог в файл
        /// </summary>
        /// <param name="torTaskId">Уникальный идентификатор задачи</param>
        /// <param name="container">IoC контейнер</param>
        /// <returns>Идентификатор файла лога</returns>
        FileInfo SaveLogFile(long torTaskId, IWindsorContainer container);
    }
}
