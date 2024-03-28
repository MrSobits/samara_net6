namespace Bars.Gkh.Decisions.Nso.Entities
{
    /// <summary>
    ///     Решение о переводе накоплений
    /// </summary>
    public class AccumulationTransferDecision : UltimateDecision
    {
        /// <summary>
        ///     Сумма накоплений переводимая на спецсчет
        /// </summary>
        public virtual decimal Decision { get; set; }
    }
}