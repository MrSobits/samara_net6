namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Предписание проверки
    /// </summary>
    public class PreceptAuditProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Проверка
        /// </summary>
        public long AuditId { get; set; }

        /// <summary>
        /// 3. Статус предписания
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 4. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 5. Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 6. Срок исполнения требований
        /// </summary>
        public DateTime? PlanExecutionDate { get; set; }

        /// <summary>
        /// 7. Краткая информация
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// 8. Статус исполнения предписания
        /// </summary>
        public int? ExecutionState { get; set; }

        /// <summary>
        /// 9. Фактическая дата исполнения
        /// </summary>
        public DateTime? ExecutionDate { get; set; }

        /// <summary>
        /// 10. Причина отмены документа
        /// </summary>
        public int? TerminationReason { get; set; }

        /// <summary>
        /// 11. Дата отмены документа
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 12. Организация, принявшая решение об отмене
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? TerminationContragent { get; set; }

        /// <summary>
        /// 13. Номер решения об отмене
        /// </summary>
        public string TerminationNumber { get; set; }

        /// <summary>
        /// 14. Дополнительная информация
        /// </summary>
        public string TerminationInfo { get; set; }
    }
}