namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Договор поставщика ресурсов с домами
    /// </summary>
    public class PublicServiceOrgContract : BaseImportableEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        [Obsolete("use PublicServiceOrgContractRealObj")]
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Поставщик ресурсов
        /// </summary>
        public virtual PublicServiceOrg PublicServiceOrg { get; set; }

        /// <summary>
        /// Дата начала обслуживания дома
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата завершения обслуживания по договору
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public virtual string ContractNumber { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public virtual DateTime? ContractDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// Основание договора с жилым домом
        /// </summary>
        public virtual ResOrgReason ResOrgReason { get; set; }

        #region Вкладка "Сведения о сроках"
        /// <summary>
        /// Срок выставления счетов к оплате, не позднее (число месяца, следующего за расчетным)
        /// </summary>
        public virtual int TermBillingPaymentNoLaterThan { get; set; }

        /// <summary>
        /// Срок оплаты, не позднее (число месяца, следующего за расчетным)
        /// </summary>
        public virtual int TermPaymentNoLaterThan { get; set; }

        /// <summary>
        /// Срок предоставления информации о поступивших задолженностях
        /// </summary>
        public virtual int DeadlineInformationOfDebt { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual int DayStart { get; set; }

        /// <summary>
        /// Период ввода показаний приборов учёта начинается с
        /// </summary>
        public virtual MonthType StartDeviceMetteringIndication { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual int DayEnd { get; set; }

        /// <summary>
        /// Период ввода показаний приборов учёта оканчивается с
        /// </summary>
        public virtual MonthType EndDeviceMetteringIndication { get; set; }
        #endregion

        #region Вкладка "Расторжение"
        /// <summary>
        /// Основание расторжения
        /// </summary>
        public virtual StopReason StopReason { get; set; }

        /// <summary>
        /// Дата расторжения
        /// </summary>
        public virtual DateTime? DateStop { get; set; }
        #endregion
    }
}
