namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using B4.DataAccess;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Дата проведения работы по решению собственника
    /// </summary>
    public class ListServiceDecisionWorkPlan : BaseEntity
    {
        /// <summary>
        /// Решение собственника
        /// </summary>
        public virtual ListServicesDecision ListServicesDecision { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Срок фактического проведения работы 
        /// </summary>
        public virtual int? FactYear { get; set; }
    }
}