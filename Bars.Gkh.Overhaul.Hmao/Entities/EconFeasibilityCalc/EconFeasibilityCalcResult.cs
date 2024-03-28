using Bars.Gkh.Entities;
using Bars.Gkh.Overhaul.Hmao.Enum;
using System;

namespace Bars.Gkh.Overhaul.Hmao.Entities
{

    public class EconFeasibilityCalcResult : BaseGkhEntity 
    {
        /// <summary>
        /// Reality Object
        /// </summary>
        public virtual RealityObject RoId { get; set; }
        /// <summary>
        /// Первый Год
        /// </summary>
        public virtual int? YearStart { get; set; }
        /// <summary>
        /// Последний год
        /// </summary>
        public virtual int? YearEnd { get; set; }
        /// <summary>
        /// Полная стоимость ремонта дома по видам работ
        /// </summary>
        public virtual  decimal? TotatRepairSumm { get; set; }
        /// <summary>
        /// Средняя стоимость квадратного метра
        /// </summary>
        public virtual LivingSquareCost SquareCost { get; set; }
        /// <summary>
        /// Стоимость жилых и нежилых помещений исходя из справочника средней стоимости
        /// </summary>
        public virtual Decimal? TotalSquareCost { get; set; }
        /// <summary>
        /// Процент стоимости работ от стоимости всех жилих и нежилых помещений 
        /// </summary>
        public virtual Decimal? CostPercent { get; set; }
        /// <summary>
        /// Решение Целесообразно или нет
        /// </summary>
        public virtual EconFeasibilityResult Decision { get; set; }

    }
}
