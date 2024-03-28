namespace Bars.GisIntegration.Base.Dictionaries.Impl
{
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Запись справочника
    /// </summary>
    public class DictionaryRecord: IDictionaryRecord
    {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Идентификатор записи справочника внешней системы
        /// </summary>
        public long ExternalId { get; private set; }

        /// <summary>
        /// Наименование записи справочника внешней системы
        /// </summary>
        public string ExternalName { get; private set; }

        /// <summary>
        /// Код записи справочника в системе ГИС
        /// </summary>
        public string GisCode { get; private set; }

        /// <summary>
        /// Guid записи справочника в системе ГИС
        /// </summary>
        public string GisGuid { get; private set; }

        /// <summary>
        /// Наименование записи справочника в системе ГИС
        /// </summary>
        public string GisName { get; private set; }

        /// <summary>
        /// Запись справочника сопоставлена
        /// записи справочника внешней системы поставлена в соответствие запись справочника ГИС
        /// </summary>
        public bool Compared => !this.GisCode.IsEmpty() && !this.GisGuid.IsEmpty();

        public DictionaryRecord(GisDictRef storableRecord)
        {
            this.Initialize(storableRecord);
        }

        private void Initialize(GisDictRef storableRecord)
        {
            this.Id = storableRecord.Id;
            this.ExternalId = storableRecord.GkhId;
            this.ExternalName = storableRecord.GkhName;
            this.GisCode = storableRecord.GisCode;
            this.GisGuid = storableRecord.GisGuid;
            this.GisName = storableRecord.GisName;
        }
    }
}
