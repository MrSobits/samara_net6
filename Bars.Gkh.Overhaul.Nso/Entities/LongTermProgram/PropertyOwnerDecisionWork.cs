namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Gkh.Entities.Dicts;

    /// <summary>
    /// Работа по КР решения собственников помещений МКД
    /// </summary>
    public class PropertyOwnerDecisionWork : BaseEntity
    {
        /// <summary>
        /// Решение собственников помещений МКД
        /// </summary>
        public virtual BasePropertyOwnerDecision Decision { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }
    }
}