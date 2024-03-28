namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.ClaimWork;
    using System;

    /// <summary>
    /// Основание ПИР для неплательщиков - юр. лиц
    /// </summary>
    public class LegalClaimWork : DebtorClaimWork
    {
        /// <summary>
        /// Адрес зоны подсудности
        /// </summary>
        public virtual RealityObject JurisdictionAddress { get; set; }

        /// <summary>
        /// Контрагент оперативного управления
        /// </summary>
        public virtual Contragent OperManagement { get; set; }

        /// <summary>
        /// Основание оперативного управления
        /// </summary>
        public virtual string OperManReason { get; set; }

        /// <summary>
        /// Дата начала оперативного управления
        /// </summary>
        public virtual DateTime? OperManDate { get; set; }

        public LegalClaimWork()
        {
            this.DebtorType = DebtorType.Legal;
        }
    }
}