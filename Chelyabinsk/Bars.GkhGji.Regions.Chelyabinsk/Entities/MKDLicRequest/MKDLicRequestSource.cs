namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Источник поступления заявки
    /// </summary>
    public class MKDLicRequestSource : BaseGkhEntity
    {
        /// <summary>
        /// Заявка
        /// </summary>
        public virtual MKDLicRequest MKDLicRequest { get; set; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        public virtual DateTime? RevenueDate { get; set; }

        /// <summary>
        /// Источник поступления
        /// </summary>
        public virtual RevenueSourceGji RevenueSource { get; set; }

        /// <summary>
        /// Исх. № источника поступления
        /// </summary>
        public virtual string RevenueSourceNumber { get; set; }

        /// <summary>
        /// Форма поступления
        /// </summary>
        public virtual RevenueFormGji RevenueForm { get; set; }

        /// <summary>
        /// Дата ССТУ
        /// </summary>
        public virtual DateTime? SSTUDate { get; set; }
    }
}