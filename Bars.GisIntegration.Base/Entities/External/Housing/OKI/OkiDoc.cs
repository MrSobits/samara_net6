namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    /// <summary>
    /// Документы ОКИ
    /// </summary>
    public class OkiDoc : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// ОКИ id
        /// </summary>
        public virtual long OkiObjectId { get; set; }
        /// <summary>
        /// Тип документа ОКИ
        /// </summary>
        public virtual OkiDocType OkiDocType  { get; set; }
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }
        /// <summary>
        /// Приложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
        /// <summary>
        /// Наименование 
        /// (вспомогательное поле, не маппится)
        /// </summary>
        public virtual string DocName { get; set; }
        /// <summary>
        /// Описание
        /// (вспомогательное поле, не маппится)
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
