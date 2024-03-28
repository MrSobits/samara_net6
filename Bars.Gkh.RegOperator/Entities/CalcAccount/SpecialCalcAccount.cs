namespace Bars.Gkh.RegOperator.Entities
{
    /// <summary>
    /// Специальный расчетный счет
    /// </summary>
    public class SpecialCalcAccount : CalcAccount
    {
        /// <summary>
        /// Признак счет активен/не активен
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}