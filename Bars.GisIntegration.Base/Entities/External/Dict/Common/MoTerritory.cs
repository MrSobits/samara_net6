namespace Bars.GisIntegration.Base.Entities.External.Dict.Common
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// МО
    /// </summary>
    public class MoTerritory : BaseEntity
    {
        /// <summary>
        /// Наименование МО
        /// </summary>
        public virtual string MoName { get; set; }
        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string Oktmo { get; set; }
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
