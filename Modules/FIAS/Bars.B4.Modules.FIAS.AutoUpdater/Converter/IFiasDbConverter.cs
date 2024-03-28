namespace Bars.B4.Modules.FIAS.AutoUpdater.Converter
{
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;

    /// <summary>
    /// Сервис конвертации данных ФИАС
    /// </summary>
    internal interface IFiasDbConverter
    {
        /// <summary>
        /// Получить записи <see cref="Fias"/> из файла
        /// </summary>
<<<<<<< HEAD
        /// <param name="dbfFilePath">Путь к *.dbf файлу</param>
        IEnumerable<AddressObjectsObject> GetFiasRecords(string dbfFilePath);
=======
        /// <param name="reader">Считыватель архива</param>
        /// <param name="isDelta">Флаг инкрементального обновления</param>
        IEnumerable<Fias> GetFiasRecords(IFiasArchiveReader reader, bool isDelta);
>>>>>>> net6

        /// <summary>
        /// Получить записи <see cref="FiasHouse"/> из файла
        /// </summary>
<<<<<<< HEAD
        /// <param name="dbfFilePath">Путь к *.dbf файлу</param>
        IEnumerable<HousesHouse> GetFiasHouseRecords(string dbfFilePath);
=======
        /// <param name="reader">Считыватель архива</param>
        /// <param name="isDelta">Флаг инкрементального обновления</param>
        IEnumerable<FiasHouse> GetFiasHouseRecords(IFiasArchiveReader reader, bool isDelta);
>>>>>>> net6
    }
}