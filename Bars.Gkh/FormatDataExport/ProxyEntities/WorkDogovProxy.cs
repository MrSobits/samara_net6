namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Работы договора на выполнение работ по капитальному ремонту
    /// </summary>
    public class WorkDogovProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Идентификатор договора на выполнение работ по капитальному ремонту
        /// </summary>
        [ProxyId(typeof(DogovorPkrProxy))]
        public long? DogovorPkrId { get; set; }

        /// <summary>
        /// 3. Работа по дому
        /// </summary>
        public int? IsHouseWork { get; set; }

        /// <summary>
        /// 4. Идентификатор работы по дому
        /// </summary>
        [ProxyId(typeof(PkrDomWorkProxy))]
        public long? PkrDomWorkId { get; set; }

        /// <summary>
        /// 5. Многоквартирный дом
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 6. Вид работ капитального ремонта
        /// </summary>
        public long? TypeWorkId { get; set; }

        /// <summary>
        /// 7. Месяц, год окончания работ в ПКР
        /// </summary>
        public DateTime? WorkEndDate { get; set; }

        /// <summary>
        /// 8. Дата начала выполнения работы
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 9. Дата окончания выполнения работы
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 10. Стоимость работы в договоре
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 11. Стоимость работы в КПР
        /// </summary>
        public decimal? KprAmount { get; set; }

        /// <summary>
        /// 12. Объём работы
        /// </summary>
        public decimal? WorkVolume { get; set; }

        /// <summary>
        /// 13. Код ОКЕИ
        /// </summary>
        public string Okei { get; set; }

        /// <summary>
        /// 14. Другая единица измерения
        /// </summary>
        public string AnotherUnit { get; set; }

        /// <summary>
        /// 15. Дополнительная информация
        /// </summary>
        public string Description { get; set; }
    }
}