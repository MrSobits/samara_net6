namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    /// <summary>
    /// Решение собственников помещений МКД (при формирования фонда КР на счете Рег.оператора)
    /// </summary>
    public class RegOpAccountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Региональный оператор
        /// </summary>
        public virtual RegOperator RegOperator { get; set; }
    }
}