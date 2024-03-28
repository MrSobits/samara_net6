namespace Bars.GisIntegration.Base.Dictionaries.Impl
{
    using System;

    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Справочник ГИС ЖКХ
    /// </summary>
    public class GisDictionary
    {
        /// <summary>
        /// Реестровый номер
        /// </summary>
        public string RegistryNumber { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Группа справочника
        /// </summary>
        public DictionaryGroup Group { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime Modified { get; set; }
    }
}