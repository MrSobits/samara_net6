namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Предметы договора ресурсоснабжения
    /// </summary>
    public class DrsoObjectProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Договор ресурсоснабжения
        /// </summary>
        public long? ResOrgContractId { get; set; }

        /// <summary>
        /// 3. Код коммунальной услуги
        /// </summary>
        [ProxyId(typeof(DictUslugaProxy))]
        public long? DictUslugaId { get; set; }

        /// <summary>
        /// 4. Тарифицируемый ресурс
        /// </summary>
        public int? CommunalResource { get; set; }

        /// <summary>
        /// 5. Зависимая схема присоединения
        /// </summary>
        public int? SchemeConnectionType { get; set; }

        /// <summary>
        /// 6. Дата начала поставки ресурса
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 7. Дата окончания поставки ресурса
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 8. Плановый объем
        /// </summary>
        public decimal? PlanVolume { get; set; }

        /// <summary>
        /// 9. Код ОКЕИ
        /// </summary>
        public string OkeiCode { get; set; }

        /// <summary>
        /// 10. Режим подачи
        /// </summary>
        public string SubmissionMode { get; set; }
    }
}