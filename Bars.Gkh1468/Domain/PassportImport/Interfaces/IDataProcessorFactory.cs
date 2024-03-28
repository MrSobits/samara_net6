namespace Bars.Gkh1468.Domain.PassportImport.Interfaces
{
    using System.Collections.Generic;
    using B4;

    /// <summary>
    /// Создает обработчик импорта на основе данных, переданных с клиента
    /// </summary>
    public interface IDataProcessorFactory
    {
        /// <summary>
        /// Создать обработчик интеграции
        /// </summary>
        /// <param name="baseParams">Данные с клиента. Создает обработчик на основе типа<see cref="ImportType1468"/>, который приходит с клиента в поле import_type</param>
        /// <param name="extraArgs">Дополнительные параметры обработки, например логгер.</param>
        /// <returns>Обработчик интеграции</returns>
        IDataProcessor CreateDataProcessor(BaseParams baseParams, IDictionary<string, object> extraArgs);
    }
}