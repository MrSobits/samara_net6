namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Членство в объединениях
    /// </summary>
    public class ManagingOrgMembership : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер свидетельства о членстве
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Официальный сайт
        /// </summary>
        public virtual string OfficialSite { get; set; }
    }
}
