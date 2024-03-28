namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using System;
    ///<summary>
    ///Справочник средней стоимости квадратного метра
    /// </summary>
    public class LivingSquareCost : BaseEntity
    {        
        /// <summary>
        /// Средняя стоимость квадратного метра
        /// </summary>
        public virtual Decimal Cost { get; set; }
        
        /// <summary>
        /// Год, на который установлена стоимость
        /// </summary>
        public virtual int Year { get; set; }
        
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
