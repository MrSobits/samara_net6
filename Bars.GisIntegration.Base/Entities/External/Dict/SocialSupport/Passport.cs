namespace Bars.GisIntegration.Base.Entities.External.Dict.SocialSupport
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// НСИ 95 Документ, удостоверяющий личность
    /// </summary>
    public class Passport : BaseEntity
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
        /// Пасспорт
        /// </summary>
        public virtual string PassportName { get; set; }
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
