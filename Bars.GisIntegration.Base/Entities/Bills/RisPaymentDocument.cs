namespace Bars.GisIntegration.Base.Entities.Bills
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Платежный документ
    /// </summary>
    public class RisPaymentDocument : BaseRisEntity
    {
        /// <summary>
        /// Ссылка на лицевой счет
        /// </summary>
        public virtual RisAccount Account { get; set; }

        /// <summary>
        /// Ссылка на сведения о платежных реквизитах организации
        /// </summary>
        public virtual RisPaymentInformation PaymentInformation { get; set; }

        /// <summary>
        /// Ссылка на адресные сведения
        /// </summary>
        public virtual RisAddressInfo AddressInfo { get; set; }

        /// <summary>
        /// Состояние платежного документа
        /// </summary>
        public virtual PaymentDocumentState State { get; set; }

        /// <summary>
        /// Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку, руб.
        /// </summary>
        public virtual decimal TotalPiecemealPaymentSum { get; set; }

        /// <summary>
        /// Дата выставления документа - счета
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Месяц рассчетного периода, за который выставлен счет
        /// </summary>
        public virtual short PeriodMonth { get; set; }

        /// <summary>
        /// Год рассчетного периода, за который выставлен счет
        /// </summary>
        public virtual short PeriodYear { get; set; }

        /// <summary>
        /// Номер платежного документа, по которому внесена плата.
        /// <remarks>Максимальная длина 30 символов.</remarks>
        /// </summary>
        public virtual string PaymentDocumentNumber { get; set; }

        /// <summary>
        /// Аванс на начало расчетного периода, руб.
        /// </summary>
        public virtual decimal AdvanceBllingPeriod { get; set; }

        /// <summary>
        /// Задолженность за предыдущие периоды, руб.
        /// </summary>
        public virtual decimal DebtPreviousPeriods { get; set; }

        /// <summary>
        /// Учетные платежи, поступившие до указанного числа расчетного периода включительно: 
        /// если атрибут даты закрытия расчетного периода (cend)
        /// </summary>
        public virtual byte PaymentsTaken { get; set; }
    }
}
