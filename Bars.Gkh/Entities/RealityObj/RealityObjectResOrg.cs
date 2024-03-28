namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Договор поставщика коммунальных услуг с жилым домом
    /// </summary>
    public class RealityObjectResOrg : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Поставщик коммунальных услуг
        /// </summary>
        public virtual SupplyResourceOrg ResourceOrg { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }
    }
}