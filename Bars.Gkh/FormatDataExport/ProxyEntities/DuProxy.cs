namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Договор управления многоквартирным домом
    /// </summary>
    public class DuProxy : IHaveId
    {
        #region DU
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор Управляющей организации
        /// </summary>
        [ProxyId(typeof(UoProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 3. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата заключения
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Дата вступления в силу
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 6. Планируемая дата окончания
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// 7. Срок действия (Месяц)
        /// </summary>
        public int? ValidityMonths { get; set; }

        /// <summary>
        /// 8. Срок действия (Год/лет)
        /// </summary>
        public int? ValidityYear { get; set; }

        /// <summary>
        /// 9. Ссылка на извещение на официальном сайте в сети «Интернет» для размещения информации о проведении торгов
        /// </summary>
        public string NoticeLink { get; set; }

        /// <summary>
        /// 10. Тип второй стороны договора
        /// </summary>
        public int? ContragentOwnerType { get; set; }

        /// <summary>
        /// 11. Контрагент второй стороны договора
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentOwnerId { get; set; }

        /// <summary>
        /// 12. Основание заключения договора
        /// </summary>
        public int? ContractFoundation { get; set; }

        /// <summary>
        /// 13. Наличие сведений о сроках по договору управления
        /// </summary>
        public bool IsTimingInfoExists =>
            (this.IsInputMeteringDeviceValuesBeginLastDay == 1 || this.InputMeteringDeviceValuesBeginDay.HasValue)
            && (this.IsInputMeteringDeviceValuesEndLastDay == 1 || this.InputMeteringDeviceValuesEndDay.HasValue)
            && (this.IsDrawingPaymentDocumentLastDay == 1 || this.InputMeteringDeviceValuesEndDay.HasValue)
            && (this.IsPaymentTermLastDay == 1 || this.PaymentTermDay.HasValue);

        /// <summary>
        /// 14. Ввод показаний по ПУ начинается в последний день месяца
        /// </summary>
        public int? IsInputMeteringDeviceValuesBeginLastDay { get; set; }

        /// <summary>
        /// 15. День месяца начала ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesBeginDay { get; set; }

        /// <summary>
        /// 16. Месяц начала ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesBeginMonth { get; set; }

        /// <summary>
        /// 17. Ввод показаний заканчивается в последний день месяца
        /// </summary>
        public int? IsInputMeteringDeviceValuesEndLastDay { get; set; }

        /// <summary>
        /// 18. День месяца окончания ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesEndDay { get; set; }

        /// <summary>
        /// 19. Месяц окончания ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesEndMonth { get; set; }

        /// <summary>
        /// 20. Платежный документ выставляется в последний день месяца
        /// </summary>
        public int? IsDrawingPaymentDocumentLastDay { get; set; }

        /// <summary>
        /// 21. День месяца выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentDay { get; set; }

        /// <summary>
        /// 22. Месяц выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentMonth { get; set; }

        /// <summary>
        /// 23. Последним днем внесения платы за ЖКУ является последний день месяца
        /// </summary>
        public int? IsPaymentTermLastDay { get; set; }

        /// <summary>
        /// 24. День месяца внесения платы за ЖКУ
        /// </summary>
        public int? PaymentTermDay { get; set; }

        /// <summary>
        /// 25. Месяц внесения платы за ЖКУ
        /// </summary>
        public int? PaymentTermMonth { get; set; }

        /// <summary>
        /// 26. Статус ДУ
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 27. Дата расторжения, прекращения действия договора управления
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 28. Причина расторжения договора
        /// </summary>
        public int? TerminationReason { get; set; }

        /// <summary>
        /// 29. Причина аннулирования
        /// </summary>
        public string CancellationReason { get; set; }

        /// <summary>
        /// 30. Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют
        /// </summary>
        public int? IsFormingApplications { get; set; }

        /// <summary>
        /// 31. Способ передачи протокола голосования собрания собственников/открытого конкурса
        /// </summary>
        public int? ProtocolTransmittingMethod { get; set; }

        /// <summary>
        /// 32. Номер извещения
        /// </summary>
        public string NoticeNumber { get; set; }
        #endregion

        #region DUCHARGE
        /// <summary>
        /// DUCHARGE 2. Статус
        /// </summary>
        public int? ChargeStatus { get; set; }

        /// <summary>
        /// DUCHARGE 4. Дата начала периода
        /// </summary>
        public DateTime? StartDatePaymentPeriod { get; set; }

        /// <summary>
        /// DUCHARGE 5. Дата окончания периода
        /// </summary>
        public DateTime? EndDatePaymentPeriod { get; set; }

        /// <summary>
        /// DUCHARGE 6. Цена за услуги, работы по управлению МКД
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// DUCHARGE 8. Причина аннулирования
        /// </summary>
        public string ChargeRevocationReason { get; set; }

        /// <summary>
        /// DUCHARGE 7. Тип размера платы
        /// </summary>
        public int? SetPaymentsFoundation { get; set; }

        /// <summary>
        /// DUCHARGE Идентификатор жилого дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? RealityObjectId { get; set; }

        #endregion

        #region DUVOTPROFILES
        /// <summary>
        /// DUVOTPROFILES 1. Файлы протоколов / протокол собрания собственников (Тип 1)
        /// </summary>
        public FileInfo ProtocolOwnersMeetingFile { get; set; }

        /// <summary>
        /// DUVOTPROFILES 1. Файлы протоколов голосований / документы решений Протокол открытого конкурса (Тип 2)
        /// </summary>
        public FileInfo ProtocolCompetitionFile { get; set; }
        #endregion
        #region DUFILES

        /// <summary>
        /// DUFILES 1. Договор управления и приложения к договору (тип 1)
        /// </summary>
        public FileInfo DuFile { get; set; }

        /// <summary>
        /// DUFILES 1. Протокол собрания собственников (тип 3)
        /// </summary>
        public FileInfo TerminationFile { get; set; }

        /// <summary>
        /// DUFILES 1. Реестр собственников, подписавших протокол (тип 4)
        /// </summary>
        public FileInfo OwnerFile { get; set; }
        #endregion

        /// <summary>
        /// DUCHARGEPROT 2. Протокол общего собрания собственников помещений в МКД
        /// об установлении размера платы за содержание жилого помещения
        /// </summary>
        public FileInfo PaymentProtocolFile { get; set; }

        #region DUOU

        /// <summary>
        /// DUCHARGE 6. Дата окончания предоставления услуг дому
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// DUOU 7. Основанием является договор управления
        /// </summary>
        public int IsContractReason { get; set; }

        /// <summary>
        /// DUOU 8. Файл с дополнительным соглашением
        /// </summary>
        public FileInfo AttachmentFile { get; set; }

        /// <summary>
        /// DUOU 9. Статус объекта управления ДУ
        /// </summary>
        public int? StatusDu { get; set; }

        /// <summary>
        /// DUOU 10. Основание исключения
        /// </summary>
        public int? ReasonTermination { get; set; }

        /// <summary>
        /// DUOU 12. Дата исключения объекта управления из ДУ
        /// </summary>
        public DateTime? ExceptionManagementDate { get; set; }

        #endregion
    }
}