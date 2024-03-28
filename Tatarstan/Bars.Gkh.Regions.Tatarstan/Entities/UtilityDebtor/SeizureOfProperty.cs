namespace Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Постановление о наложении ареста на имущество
    /// </summary>
    public class SeizureOfProperty : DocumentClw
    {
        //2 поля дублируют аналогичные в UtilityDebtorClaimWork

        /// <summary>
        /// Абонент
        /// </summary>
        public virtual string AccountOwner { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual OwnerType OwnerType { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string AccountOwnerBankDetails { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual string Year { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Подномер
        /// </summary>
        public virtual string SubNumber { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual string Document { get; set; }

        /// <summary>
        /// Дата вручения
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Аннулировано
        /// </summary>
        public virtual bool IsCanceled { get; set; }

        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public virtual string CancelReason { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual string Official { get; set; }

        /// <summary>
        /// Местонахождение
        /// </summary>
        public virtual string Location { get; set; }

        /// <summary>
        /// Взыскатель
        /// </summary>
        public virtual string Creditor { get; set; }

        /// <summary>
        /// Реквизиты
        /// </summary>
        public virtual string BankDetails { get; set; }

        /// <summary>
        /// Орган, вынесший постановление (Подразделение ОСП)
        /// дублирует поле из ExecutoryProcess
        /// </summary>
        public virtual JurInstitution JurInstitution { get; set; }
    }
}