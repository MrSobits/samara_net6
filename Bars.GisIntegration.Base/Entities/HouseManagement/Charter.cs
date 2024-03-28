namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Устав
    /// </summary>
    public class Charter : BaseRisEntity
    {
        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNum { get; set; }

        /// <summary>
        /// Дата регистрации ТСЖ/кооператива
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Период ввода показаний по ПУ (День месяца начала ввода показаний по ПУ)
        /// </summary>
        public virtual int? PeriodMeteringStartDate { get; set; }

        /// <summary>
        /// Период ввода показаний по ПУ (День месяца начала ввода показаний по ПУ) - текущего месяца
        /// </summary>
        public virtual bool? PeriodMeteringStartDateThisMonth { get; set; }

        /// <summary>
        /// Период ввода показаний по ПУ (День месяца окончания ввода показаний по ПУ)
        /// </summary>
        public virtual int? PeriodMeteringEndDate { get; set; }

        /// <summary>
        /// Период ввода показаний по ПУ (День месяца окончания ввода показаний по ПУ) - текущего месяца
        /// </summary>
        public virtual bool? PeriodMeteringEndDateThisMonth { get; set; }

        /// <summary>
        /// Период ввода показаний по ПУ (Последний день месяца)
        /// </summary>
        public virtual bool PeriodMeteringLastDay { get; set; }

        /// <summary>
        /// Дата выставления платежных документов (День месяца)
        /// </summary>
        public virtual int? PaymentDateStartDate { get; set; }

        /// <summary>
        /// Дата выставления платежных документов (Последний день месяца)
        /// </summary>
        public virtual bool PaymentDateLastDay { get; set; }

        /// <summary>
        /// Состав органов управления
        /// </summary>
        public virtual string Managers { get; set; }

        /// <summary>
        /// Председатель правления
        /// </summary>
        public virtual RisInd Head { get; set; }

        /// <summary>
        /// Документ устава
        /// </summary>
        public virtual Attachment Attachment { get; set; }

        /// <summary>
        /// Утвердить устав
        /// </summary>
        public virtual bool ApprovalCharter { get; set; }

        /// <summary>
        /// Продление устава
        /// </summary>
        public virtual bool RollOverCharter { get; set; }

        /// <summary>
        /// Прекращение действия устава (дата)
        /// </summary>
        public virtual bool TerminateCharterDate { get; set; }

        /// <summary>
        /// Прекращение действия устава (причина)
        /// </summary>
        public virtual bool TerminateCharterReason { get; set; }

        /// <summary>
        /// Дата выставления платежных документов - Текущего месяца
        /// </summary>
        public virtual bool? ThisMonthPaymentDocDate { get; set; }

        /// <summary>
        /// Периоды внесения платы за ЖКУ: День месяца
        /// </summary>
        public virtual byte? PaymentServicePeriodDate { get; set; }

        /// <summary>
        /// Периоды внесения платы за ЖКУ - этого месяца (если false - следующего месяца)
        /// </summary>
        public virtual bool? ThisMonthPaymentServiceDate { get; set; }

        /// <summary>
        /// Управление многоквартирным домом осуществляется управляющей организацией по договору управления
        /// </summary>
        public virtual bool IsManagedByContract { get; set; }
    }
}