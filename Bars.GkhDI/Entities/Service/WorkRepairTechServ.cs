namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работа по ТО услуги ремонт
    /// </summary>
    public class WorkRepairTechServ : BaseGkhEntity
    {
        /// <summary>
        /// Услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Работа по ТО
        /// </summary>
        public virtual WorkTo WorkTo { get; set; }

    }
}
