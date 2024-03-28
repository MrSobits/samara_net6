namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// План работ по содержанию и ремонту
    /// </summary>
    public class PlanWorkServiceRepair : BaseGkhEntity
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
