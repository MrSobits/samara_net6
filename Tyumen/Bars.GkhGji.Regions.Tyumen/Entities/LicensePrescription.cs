namespace Bars.GkhGji.Regions.Tyumen.Entities
{
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using System;

    /// <summary>
    /// Уведомление для заявителя
    /// </summary>
    public class LicensePrescription : BaseEntity
    {
        /// <summary>
        /// Код.
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата постановления
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLawGji { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public virtual DateTime? ActualDate { get; set; }

        /// <summary>
        /// Скан
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Обжалование
        /// </summary>
        public virtual YesNoNotSet YesNoNotSet { get; set; }

        /// <summary>
        /// Санкция
        /// </summary>
        public virtual SanctionGji SanctionGji { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal Penalty { get; set; }

        /// <summary>
        /// Родительская запись
        /// </summary>
        public virtual ManOrgContractRealityObject MorgContractRO { get; set; }



    }
}