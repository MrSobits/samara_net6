namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Запись справочника
    /// </summary>
    public class DictEntry
    {
        /// <summary>
        /// Уникальный идентификатор записи справочника
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Значение записи справочника
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        /// Дата изменения записи
        /// </summary>
        [JsonIgnore]
        public DateTime EditDate { get; set; }
    }
}