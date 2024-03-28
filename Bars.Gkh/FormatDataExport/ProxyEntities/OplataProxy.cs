namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Оплата ЖКУ
    /// </summary>
    public class OplataProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код оплаты
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный код лицевого счета в системе отправителя
        /// </summary>
        [ProxyId(typeof(KvarProxy))]
        public long? KvarId { get; set; }

        /// <summary>
        /// 3. Тип операции
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// 4. Уникальный идентификатор распоряжения (уникальный номер платежа)
        /// </summary>
        public string OplataPackNumber { get; set; }

        /// <summary>
        /// 5. Дата оплаты
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 6. Дата учета
        /// </summary>
        public DateTime? OperationDate { get; set; }

        /// <summary>
        /// 7. Номер распоряжения
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 8. Сумма оплаты
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 9. Источник оплаты
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 10. Месяц, за который произведена оплата
        /// </summary>
        public DateTime? Month { get; set; }

        /// <summary>
        /// 11. Уникаьный код пачки оплат
        /// </summary>
        [ProxyId(typeof(OplataPackProxy))]
        public long? OplataPackId { get; set; }

        /// <summary>
        /// 12. Исполнитель
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 13. Расчетный счет получателя платежа
        /// </summary>
        [ProxyId(typeof(ContragentRschetProxy))]
        public long? ContragentRschetId { get; set; }

        /// <summary>
        /// 14. Платежный документ
        /// </summary>
        [ProxyId(typeof(EpdProxy))]
        public long? EpdId { get; set; }

        /// <summary>
        /// 15. Уникальный идентификатор плательщика (физ. лицо)
        /// </summary>
        [ProxyId(typeof(IndProxy))]
        public long? PayerIndId { get; set; }

        /// <summary>
        /// 16. Уникальный идентификатор плательщика (юр. лицо)
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? PayerLegalId { get; set; }

        /// <summary>
        /// 17. Наименование плательщика
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// 18. Назначение платежа
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// 19. Произвольный комментарий
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 20. Статус оплаты
        /// </summary>
        public string State => "1"; // Действует

        /// <summary>
        /// 21. Дата аннулирования
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 22. Причина аннулирования
        /// </summary>
        public string TerminationReason { get; set; }

        /// <summary>
        /// KVISOL 3. Результат квитирования
        /// </summary>
        public int? KvisolResult { get; set; }

        /// <summary>
        /// KVISOL 5. Статус квитирования
        /// </summary>
        public int? KvisolState => 1;

        /// <summary>
        /// KVISOL 6. Сумма квитирования (в копейках)
        /// </summary>
        public decimal? KvisolSum { get; set; }

        /// <summary>
        /// KVISOLSERVICE 3. Тип начисления
        /// </summary>
        public int? KvisolServiceType => 2;

        /// <summary>
        /// KVISOLSERVICE 5. Начисление по капитальному ремонту
        /// </summary>
        public long? EpdCapitalId { get; set; }
    }
}