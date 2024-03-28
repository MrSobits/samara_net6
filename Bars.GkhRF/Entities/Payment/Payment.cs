namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Оплата
    /// </summary>
    public class Payment : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? ChargePopulationCr { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? ChargePopulationHireRf { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? ChargePopulationCr185 { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? ChargePopulationBldRepair { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? PaidPopulationCr { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? PaidPopulationHireRf { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? PaidPopulationCr185 { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual decimal? PaidPopulationBldRepair { get; set; }
    }
}