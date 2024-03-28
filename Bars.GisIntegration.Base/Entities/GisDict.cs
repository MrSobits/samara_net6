namespace Bars.GisIntegration.Base.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Справочник. Содержит свойства для связи справочников ГИС со справочниками систем источников данных (пока свойство ActionCode)
    /// </summary>
    public class GisDict : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Идентификатор IGisIntegrationDictAction
        /// </summary>
        public virtual string ActionCode { get; set; }

        /// <summary>
        /// Номер справочника в Nsi
        /// </summary>
        public virtual string NsiRegistryNumber { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        //public virtual DateTime? DateIntegration { get; set; }

        /// <summary>
        /// Группа справочника
        /// </summary>
        public virtual DictionaryGroup? Group { get; set; }

        /// <summary>
        /// Дата последнего сопоставления записей справочника
        /// </summary>
        public virtual DateTime? LastRecordsCompareDate { get; set; }

        /// <summary>
        /// Состояние справочника
        /// </summary>
        public virtual DictionaryState State { get; set; }
    }
}