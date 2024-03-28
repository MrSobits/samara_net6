namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Работа дома ПКР (pkrdomwork.csv)
    /// </summary>
    public class PkrDomWorkProxy : IHaveId
    {
        /// <summary>
        /// 1. Код работы по дому
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Идентификатор дома
        /// </summary>
        [ProxyId(typeof(PkrDomProxy))]
        public long? PkrDomId { get; set; }

        /// <summary>
        /// 3. Вид работы капитального ремонта
        /// </summary>
        [ProxyId(typeof(WorkKprTypeProxy))]
        public long? WorkKprTypeId { get; set; }

        /// <summary>
        /// 4. Начало выполнения работ
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 5. Окончание выполнения работ
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 6. Средства Фонда ЖКХ
        /// </summary>
        public decimal? FundResourses { get; set; }

        /// <summary>
        /// 7. Бюджет субъекта РФ
        /// </summary>
        public decimal? SubjectBudget { get; set; }

        /// <summary>
        /// 8. Местный бюджет
        /// </summary>
        public decimal? LocalBudget { get; set; }

        /// <summary>
        /// 9. Средства собственников
        /// </summary>
        public decimal? OwnerBudget { get; set; }

        /// <summary>
        /// 10. Удельная стоимость работы
        /// </summary>
        public decimal? UnitCost { get; set; }
        
        /// <summary>
        /// 11. Предельная стоимость работы
        /// </summary>
        public decimal? MarginalCost { get; set; }
    }
}