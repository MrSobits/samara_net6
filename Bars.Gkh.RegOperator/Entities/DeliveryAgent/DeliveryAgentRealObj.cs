
using System;
using Bars.Gkh.Entities;

namespace Bars.Gkh.RegOperator.Entities
{
    /// <summary>
    /// Жилой дом агента доставки
    /// </summary>
    public class DeliveryAgentRealObj : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Агент доставки
        /// </summary>
        public virtual DeliveryAgent DeliveryAgent { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата начала действия договора
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}