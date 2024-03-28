using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhCr.Entities;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    /// <summary>
    /// Связь Предельная стоимость и Работа
    /// </summary>
    public class CostLimitTypeWorkCr : BaseEntity
    {
        /// <summary>
        /// Предельная стоимость
        /// </summary>
        public virtual CostLimit CostLimit { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual string Year { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public virtual decimal Cost { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
