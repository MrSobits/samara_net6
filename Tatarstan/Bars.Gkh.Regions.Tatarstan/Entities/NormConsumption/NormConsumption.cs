namespace Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption
{
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Нормативы потребления
    /// </summary>
    public class NormConsumption : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual PeriodNormConsumption Period { get; set; }

        /// <summary>
        /// Вид номратива потребления
        /// </summary>
        public virtual NormConsumptionType Type { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}