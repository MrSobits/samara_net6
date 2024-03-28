namespace Bars.Gkh.SystemDataTransfer
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Интерфейс провайдера переноса данных
    /// </summary>
    public interface IDataTransferProvider
    {
        /// <summary>
        /// Событие конца импорта секции
        /// </summary>
        event Action<string, bool> OnSectionImportDone;

        /// <summary>
        /// Получить экспортируемые сущности
        /// </summary>
        /// <param name="typeNames">Имена сущностей для экспорта</param>
        /// <param name="exportDependencies">Экспортировать зависимые</param>
        /// <returns>Данные для экспорта, сериализованные в бинарный формат</returns>
        Stream Export(IEnumerable<string> typeNames = null, bool exportDependencies = true);

        /// <summary>
        /// Метод импорта сущностей
        /// </summary>
        /// <param name="stream">Поток (.zip архив с json'ами внутри)</param>
        void Import(Stream stream);

        /// <summary>
        /// Метод возвращает типы по слоям интеграции для последовательной интеграции
        /// </summary>
        IList<Type[]> GetLayers(IEnumerable<string> typeNames = null, bool exportDependencies = true);
    }
}