namespace Bars.GkhGji.Entities
{
    using System;
    using B4.DataAccess;
    using Dict;
    using Gkh.Entities;

    /// <summary>
    /// Цели контрагента
    /// </summary>
    public class ContragentAuditPurpose : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Цель
        /// </summary>
        public virtual AuditPurposeGji AuditPurpose { get; set; }

        /// <summary>
        /// Дата прошлой проверки 
        /// </summary>
        public virtual DateTime? LastInspDate { get; set; }
    }
}