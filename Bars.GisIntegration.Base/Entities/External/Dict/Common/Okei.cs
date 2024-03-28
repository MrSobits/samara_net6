namespace Bars.GisIntegration.Base.Entities.External.Dict.Common
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// ОКЕИ
    /// </summary>
    public class Okei : BaseEntity
    {
        /// <summary>
        /// Наименование ОКЕИ
        /// </summary>
        public virtual string OkeiName { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual long OkeiCode { get; set; }

        /// <summary>
        /// Условное национальное обозначение 
        /// </summary>
        public virtual string NationalSymbol { get; set; }

        /// <summary>
        /// Условное международное обозначение 
        /// </summary>
        public virtual string WorldSymbol { get; set; }

        /// <summary>
        /// Условный национальный код 
        /// </summary>
        public virtual string NationalCode { get; set; }

        /// <summary>
        /// Условный международный код 
        /// </summary>
        public virtual string WorldCode { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

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
