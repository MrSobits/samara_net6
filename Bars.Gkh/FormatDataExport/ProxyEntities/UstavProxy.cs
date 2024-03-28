namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Уставы
    /// </summary>
    public class UstavProxy : IHaveId
    {
        #region USTAV
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор Управляющей организации/ТСЖ
        /// </summary>
        [ProxyId(typeof(UoProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 3. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Наличие сведений о сроках по уставу
        /// </summary>
        public int? IsTimingInfoExists { get; set; }

        /// <summary>
        /// 5. Ввод показаний по ПУ начинается в последний день месяца
        /// </summary>
        public int? IsInputMeteringDeviceValuesLastDay { get; set; }

        /// <summary>
        /// 6. День месяца начала ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesBeginDay { get; set; }
        
        /// <summary>
        /// 7. Месяц начала ввода показаний
        /// </summary>
        public int? ThisMonthInputMeteringDeviceValuesBeginDate { get; set; }

        /// <summary>
        /// 8. Ввод показаний заканчивается в последний день месяца
        /// </summary>
        public int? IsLastDayMeteringDeviceValuesBeginDate { get; set; }
        
        /// <summary>
        /// 9. День месяца окончания ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesEndDay { get; set; }

        /// <summary>
        /// 10. Месяц окончания ввода показаний
        /// </summary>
        public int? ThisMonthInputMeteringDeviceValuesEndDate { get; set; }

        /// <summary>
        /// 11. Платежный документ выставляется в последний день месяца
        /// </summary>
        public int? IsDrawingPaymentDocumentLastDay { get; set; }

        /// <summary>
        /// 12. День месяца выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentDay { get; set; }
        
        /// <summary>
        /// 13. Месяц выставления платежных документов
        /// </summary>
        public int? ThisMonthPaymentDocDate { get; set; }

        /// <summary>
        /// 14. Оплата за ЖКУ должна быть внесена в последний день месяца
        /// </summary>
        public int? IsPaymentServicePeriodLastDay { get; set; }
        
        /// <summary>
        /// 15. День месяца внесения платы за ЖКУ
        /// </summary>
        public int? PaymentServicePeriodDay { get; set; }
        
        /// <summary>
        /// 16. Месяц внесения платы за ЖКУ
        /// </summary>
        public int? ThisMonthPaymentServiceDate { get; set; }

        /// <summary>
        /// 17. Статус устава
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 18. Причина аннулирования
        /// </summary>
        public string TerminateReason { get; set; }

        /// <summary>
        /// 19. Дата расторжения, прекращения действия устава
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 20. Причина прекращения действия устава (расторжения)
        /// </summary>
        public string ContractStopReason { get; set; }

        /// <summary>
        /// 21. Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют
        /// </summary>
        public int? IsFormingApplications { get; set; }

        /// <summary>
        /// 22. Протокол, содержащий решение об утверждении устава отсутствует
        /// </summary>
        public int? IsProtocolContainsDecision{ get; set; }
        #endregion

        #region USTAVCHARGE
        /// <summary>
        /// USTAVCHARGE Тип договора управляющей организации
        /// </summary>
        public TypeContractManOrg TypeContract { get; set; }

        /// <summary>
        /// USTAVCHARGE 3. Статус
        /// </summary>
        public int? ChargeStatus { get; set; }

        /// <summary>
        /// USTAVCHARGE 4. Дата начала периода
        /// </summary>
        public DateTime? StartDatePaymentPeriod { get; set; }

        /// <summary>
        /// USTAVCHARGE 5. Дата окончания периода
        /// </summary>
        public DateTime? EndDatePaymentPeriod { get; set; }

        /// <summary>
        /// USTAVCHARGE 6. Размер обязательных платежей членов ТСЖ
        /// </summary>
        public decimal? CompanyReqiredPaymentAmount { get; set; }

        /// <summary>
        /// USTAVCHARGE 7. Размер платы за содержание и ремонт помещений
        /// </summary>
        public decimal? ReqiredPaymentAmount { get; set; }

        /// <summary>
        /// USTAVCHARGE 8. Для всех управляемых объектов
        /// </summary>
        public int? ForAllManagedObjects{ get; set; }
        
        /// <summary>
        /// USTAVCHARGE 9. Объект управления устава
        /// </summary>
        public int? ControlObject{ get; set; }
        
        /// <summary>
        /// USTAVCHARGE Протокол собрания - Платежи/взносы собственников, не являющихся членами товарищества, кооператива
        /// </summary>
        public FileInfo PaymentProtocolFile { get; set; }

        /// <summary>
        /// USTAVCHARGE Идентификатор жилого дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? RealityObjectId { get; set; }
        #endregion

        #region USTAVFILES
        /// <summary>
        /// USTAVFILES 1. Вложение устава Протокол ОСС (Тип 1)
        /// </summary>
        public FileInfo OssFile { get; set; }

        /// <summary>
        /// USTAVFILES 1. Вложение устава Документы устава (Тип 2)
        /// </summary>
        public FileInfo UstavFile { get; set; }
        #endregion

        #region USTAVOU
        /// <summary>
        /// USTAVOU 4. Дата начала предоставления услуг дому
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// USTAVOU 5. Дата окончания предоставления услуг дому
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// USTAVOU 6. Статус объекта управления устава
        /// </summary>
        public int? StatusOu { get; set; }

        /// <summary>
        /// USTAVOU 7. Основанием является договор управления
        /// </summary>
        public int? IsContractReason { get; set; }

        /// <summary>
        /// USTAVOU 8. Файл с протоколом собрания собственников
        /// </summary>
        public FileInfo ProtocolMeetingFile { get; set; }

        /// <summary>
        /// USTAVOU 9. Управление многоквартирным домом осуществляется управляющей организацией по договору управления
        /// </summary>
        public int? IsManagementContract { get; set; }

        /// <summary>
        /// USTAVOU 10. Основание исключения
        /// </summary>
        public int? IsExclusionReason { get; set; }

        /// <summary>
        /// USTAVOU 11. Файл с протоколом собрания собственников
        /// </summary>
        public FileInfo ProtocolMeetingExcludeFile { get; set; }

        /// <summary>
        /// USTAVOU 12. Дата исключения
        /// </summary>
        public DateTime? ExcludeDate { get; set; }

        #endregion
    }
}