namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Расчетный счет фонда капитального ремонта
    /// </summary>
    public class RegopSchetProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Тип счета
        /// </summary>
        public int? TypeAccount { get; set; }

        /// <summary>
        /// Региональный оператор капитального ремонта / Контрагент
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 3. Региональный оператор капитального ремонта
        /// </summary>
        public long? RegopContragentId => this.TypeAccount == 1 ? this.ContragentId : null;

        /// <summary>
        /// 4. Контрагент
        /// </summary>
        public long? SpecialContragentId => this.TypeAccount == 2 ? this.ContragentId : null;

        /// <summary>
        /// 5. Расчетный счет
        /// </summary>
        [ProxyId(typeof(ContragentRschetProxy))]
        public long? ContragentRschetId { get; set; }

        /// <summary>
        /// 6. Статус
        /// </summary>
        public int Status { get; set; }
    }
}