namespace Bars.Gkh.SystemDataTransfer.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.Gkh.SystemDataTransfer.Enums;

    /// <summary>
    /// Интерфейс сервиса работы с импортом/экспортом системы
    /// </summary>
    public interface ISystemIntegrationService
    {
        /// <summary>
        /// Метод запускает процесс интерации с внешней системой
        /// </summary>
        IDataResult RunIntegration(BaseParams baseParams);

        /// <summary>
        /// Оповестить о взятии экспорта в работу
        /// </summary>
        IDataResult NotifyStartExport(Guid guid);

        /// <summary>
        /// Оповестить о взятии импорта в работу
        /// </summary>
        IDataResult NotifyStartImport(Guid guid);

        /// <summary>
        /// Поставить в очередь задачу экспорта данных
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        /// <param name="typeNames">"Экспортируемые сущности</param>
        /// <param name="exportDependencies">Выгружать зависимости</param>
        IDataResult CreateExportTask(Guid guid, IEnumerable<string> typeNames = null, bool exportDependencies = true);

        /// <summary>
        /// Поставить в очередь задачу импорта данных
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        IDataResult CreateImportTask(Guid guid, Stream stream);

        /// <summary>
        /// Метод отправляет поток во внешнюю системы
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        /// <param name="streamResult">Результат экспорта</param>
        IDataResult SendExportResult(Guid guid, IDataResult<Stream> streamResult);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        /// <param name="result">Результат импорта</param>
        /// <returns></returns>
        IDataResult SendImportResult(Guid guid, IDataResult result);

        /// <summary>
        /// Обработать входящее уведомление
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        /// <param name="operationType">Вид операции</param>
        /// <param name="result">Информация в сообщении</param>
        IDataResult ProcessNotify(Guid guid, DataTransferOperationType operationType, IDataResult result);

        /// <summary>
        /// Обработать результат импорта
        /// </summary>
        /// <param name="guid">Идентификатор сессии</param>
        /// <param name="name">Имя секции</param>
        /// <param name="success">Успешность</param>
        /// <param name="notifyExternalSystem">Послать уведомление внешней системе</param>
        /// <returns></returns>
        IDataResult HandleSectionImportState(Guid guid, string name, bool success, bool notifyExternalSystem);
    }
}