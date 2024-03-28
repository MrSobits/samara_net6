namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Классы энергетической эффективности
    /// </summary>
    public class EnergyEfficiencyClasses : BaseGkhDict
    {
        /// <summary>
        /// Обозначение класса энергетической эффективности
        /// </summary>
        public virtual string Designation { get; set; }

        /// <summary>
        /// Величина отклонения значения фактического удельного годового расхода энергетических ресурсов от базового уровня
        /// </summary>
        public virtual string DeviationValue { get; set; }
    }
}
