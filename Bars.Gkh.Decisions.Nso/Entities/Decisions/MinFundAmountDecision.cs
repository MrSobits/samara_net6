namespace Bars.Gkh.Decisions.Nso.Entities
{
    /// <summary>
    ///     Размер минимального фонда на КР 
    /// </summary>
    public class MinFundAmountDecision : UltimateDecision
    {
        /// <summary>
        ///     Текущее значение
        /// </summary>
        public virtual decimal Decision { get; set; }

        /// <summary>
        ///     Минимальное значение
        /// </summary>
        public virtual decimal Default { get; set; }
    }
}