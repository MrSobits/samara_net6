namespace Bars.GisIntegration.Base.Entities.Dictionaries
{
    using System;

    using B4.DataAccess;

    /// <summary>
    /// Справочники
    /// </summary>
    public class DictInfo : BaseEntity
    {
        /// <summary>
        /// Реестровый номер справочника
        /// </summary>
        public virtual int RegistryNumber { get;  set; }

        /// <summary>
        /// Наименование справочника
        /// </summary>
        public virtual string Name { get;  set; }

        /// <summary>
        /// Дата и время последнего изменения справочника.
        /// </summary>
        public virtual DateTime Modified { get;  set; }

        /// <summary>
        /// Дата последнего запроса справочника
        /// </summary>
        public virtual DateTime LastRequest { get;  set; }

        /// <summary>
        /// Последний ответ в формате JSON
        /// </summary>
        public virtual string RawReply { get;  set; }
    }
}
