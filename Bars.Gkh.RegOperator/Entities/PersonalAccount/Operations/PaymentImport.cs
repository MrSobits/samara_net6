namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    ///Импортируемые оплаты
    /// </summary>
    public class PaymentImport : BaseImportableEntity
    {
        /// <summary>
        /// Операция оплаты
        /// </summary>
        public virtual PaymentOperationBase PaymentOperation { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount BasePersonalAccount { get; set; }

        /// <summary>
        /// Дата операции
        /// </summary>
        public virtual DateTime PaymentDate { get; set; }

        /// <summary>
        /// Сумма по тарифу решения
        /// </summary>
        public virtual decimal TariffDecisionPayment { get; set; }

        /// <summary>
        /// Сумма по базовому тарифу
        /// </summary>
        public virtual decimal TariffPayment { get; set; }

        /// <summary>
        /// Сумма по пени
        /// </summary>
        public virtual decimal PenaltyPayment { get; set; }

        /// <summary>
        /// Тип оплаты
        /// </summary>
        public virtual ImportPaymentType PaymentType { get; set; }

        /// <summary>
        /// Номер документа/реестра
        /// </summary>
        public virtual string RegistryNum { get; set; }

        /// <summary>
        /// Дата документа/реестра
        /// </summary>
        public virtual DateTime RegistryDate { get; set; }
    }
}