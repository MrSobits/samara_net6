using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities
{

    /// <summary>
    /// Виды рисков
    /// </summary>
    public class CSCalculationRow : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual CSCalculation CSCalculation { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual decimal Value { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string DisplayValue { get; set; }
    }
}