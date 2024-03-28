namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Работа по КР решения собственников помещений МКД
    /// </summary>
    public class PropertyOwnerDecisionWork : BaseImportableEntity
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