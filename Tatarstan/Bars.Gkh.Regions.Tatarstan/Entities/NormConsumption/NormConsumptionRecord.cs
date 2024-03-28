namespace Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Записи нормативов потребления
    /// </summary>
    public class NormConsumptionRecord : BaseGkhEntity
    {
        /// <summary>
        /// Норматив потребления
        /// </summary>
        public virtual NormConsumption NormConsumption { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}