namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Протокол проверки
    /// </summary>
    public class ProtocolAuditProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Проверка
        /// </summary>
        public long AuditId { get; set; }

        /// <summary>
        /// 3. Статус протокола
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 4. Номер протокола
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 5. Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 6. Краткая информация
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 7. Статус исполнения протокола
        /// </summary>
        public int? ExecutionState { get; set; }

        /// <summary>
        /// 8. Фактическая дата исполнения
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// 9. Причина отмены документа
        /// </summary>
        public int? TerminationReason { get; set; }

        /// <summary>
        /// 10. Дата отмены документа
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 11. Организация, принявшая решение об отмене
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? TerminationContragent { get; set; }

        /// <summary>
        /// 12. Номер решения об отмене
        /// </summary>
        public string TerminationNumber { get; set; }

        /// <summary>
        /// 13. Дополнительная информация
        /// </summary>
        public string TerminationInfo { get; set; }
    }
}