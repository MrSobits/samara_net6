namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Базовый класс договоров управления
    /// </summary>
    public class ManOrgBaseContract : BaseGkhEntity
    {
        #region Constants

        /// <summary>
        /// Текст-заглушка вместо названия управляющей компании при непосредственном управлении
        /// </summary>
        public const string DirectManagementText = "Непосредственное управление";
        /// <summary>
        /// Текст-заглушка вместо названия управляющей компании при непосредственном управлении с договором
        /// </summary>
        public const string DirectManagementWithContractText = "Непосредственное управление с договором на оказание услуг";

        #endregion

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Тип договора управляющей организации
        /// </summary>
        public virtual TypeContractManOrg TypeContractManOrgRealObj { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата начала управления
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания управления
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Плановая дата окончания
        /// </summary>
        public virtual DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Основание расторжения
        /// </summary>
        public virtual string TerminateReason { get; set; }

        /// <summary>
        /// Дата расторжения
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Нехранимое поле Идентификатор жилого дома
        /// </summary>
        public virtual long RealityObjectId { get; set; }

        /// <summary>
        /// Основание прекращения обслуживания
        /// </summary>
        public virtual ContractStopReasonEnum ContractStopReason { get; set; }

        /// <summary>
        /// Периоды внесения платы за ЖКУ: День месяца
        /// </summary>
        public virtual byte? PaymentServicePeriodDate { get; set; }

        /// <summary>
        /// Периоды внесения платы за ЖКУ - Последний день месяца
        /// </summary>
        public virtual bool IsLastDayPaymentServicePeriodDate { get; set; }

        /// <summary>
        /// День выставления платежных документов - этого месяца (если false - следующего месяца)
        /// </summary>
        public virtual bool ThisMonthPaymentDocDate { get; set; }

        /// <summary>
        /// Периоды внесения платы за ЖКУ - этого месяца (если false - следующего месяца)
        /// </summary>
        public virtual bool ThisMonthPaymentServiceDate { get; set; }

        /// <summary>
        /// День месяца начала ввода показаний по приборам учета - этого месяца (если false - следующего месяца)
        /// </summary>
        public virtual bool ThisMonthInputMeteringDeviceValuesBeginDate { get; set; }

        /// <summary>
        /// День месяца окончания ввода показаний по приборам учета - этого месяца (если false - следующего месяца)
        /// </summary>
        public virtual bool ThisMonthInputMeteringDeviceValuesEndDate { get; set; }

        /// <summary>
        /// Сведения о плате - дата начала периода
        /// </summary>
        public virtual DateTime? StartDatePaymentPeriod { get; set; }

        /// <summary>
        /// Сведения о плате - дата окончания периода
        /// </summary>
        public virtual DateTime? EndDatePaymentPeriod { get; set; }

        /// <summary>
        /// Сведения о плате - Основание установления размера платы за содержание жилого помещения
        /// </summary>
        public virtual ManOrgSetPaymentsOwnersFoundation? SetPaymentsFoundation { get; set; }

        /// <summary>
        /// Сведения о плате - Причина аннулирования
        /// </summary>
        public virtual string RevocationReason { get; set; }

        /// <summary>
        /// День месяца начала ввода показаний по приборам учета - Последний день месяца
        /// </summary>
        public virtual bool IsLastDayMeteringDeviceValuesBeginDate { get; set; }

        /// <summary>
        /// День месяца окончания ввода показаний по приборам учета - Последний день месяца
        /// </summary>
        public virtual bool IsLastDayMeteringDeviceValuesEndDate { get; set; }

        /// <summary>
        /// День выставления платежных документов - Последний день месяца
        /// </summary>
        public virtual bool IsLastDayDrawingPaymentDocument { get; set; }
    }
}