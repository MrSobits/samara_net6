using System;

namespace GisGkhLibrary.Entities
{
    /// <summary>
    /// Запись о словаре
    /// </summary>
    public class DictionaryInfo
    {
        /// <summary>
        /// Реестровый номер справочника
        /// </summary>
        public int RegistryNumber { get; internal set; }

        /// <summary>
        /// Наименование справочника
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Дата и время последнего изменения справочника.
        /// </summary>
        public DateTime Modified { get; internal set; }
    }
}
