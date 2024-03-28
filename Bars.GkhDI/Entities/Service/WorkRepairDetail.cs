namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// ППР детализация работ
    /// </summary>
    public class WorkRepairDetail : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Работа ППР
        /// </summary>
        public virtual WorkPpr WorkPpr { get; set; }
    }
}
