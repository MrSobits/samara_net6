namespace Bars.Gkh.Overhaul.Entities
{
    using System;
    using Bars.Gkh.Entities;

    using Enum;

    /// <summary>
    /// Размер взноса на кр
    /// </summary>
    public class Paysize : BaseImportableEntity
    {
        /// <summary>
        /// Показатель
        /// </summary>
        public virtual TypeIndicator Indicator { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}