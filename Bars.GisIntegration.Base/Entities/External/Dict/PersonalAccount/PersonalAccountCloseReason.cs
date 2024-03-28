namespace Bars.GisIntegration.Base.Entities.External.Dict.PersonalAccount
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// НСИ 22. Причина закрытия счета
    /// </summary>
    public class PersonalAccountCloseReason : BaseEntity
    {
        /// <summary>
        /// Код записи справочника
        /// </summary>
        public virtual string DictCode { get; set; }
        /// <summary>
        /// Идентификатор в ГИС ЖКХ
        /// </summary>
        public virtual string GisGuid { get; set; }
        /// <summary>
        /// Наименование причины закрытия счета
        /// </summary>
        public virtual string Value { get; set; }
        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
