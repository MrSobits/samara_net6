namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Претензия к управляющей организации
    /// </summary>
    public class ManagingOrgClaim : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Сумма претензии
        /// </summary>
        public virtual decimal? AmountClaim { get; set; }

        /// <summary>
        /// Содержание претензии
        /// </summary>
        public virtual string ContentClaim { get; set; }

        /// <summary>
        /// Дата претензии
        /// </summary>
        public virtual DateTime? DateClaim { get; set; }
    }
}
