namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// План мер по снижению расходов
    /// </summary>
    public class PlanReductionExpense : BaseGkhEntity
    {
        /// <summary>
        /// Объект в управление
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }
    }
}
