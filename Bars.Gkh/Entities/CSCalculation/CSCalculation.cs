using Bars.B4.DataAccess;
using System;
using System.Collections.Generic;

namespace Bars.Gkh.Entities
{   

    /// <summary>
    /// Виды рисков
    /// </summary>
    public class CSCalculation : BaseEntity
    {
        //private readonly IList<CSCalculationRow> calculatedVariables;
        //public CSCalculation()
        //{
        //    this.calculatedVariables = new List<CSCalculationRow>();
        //}

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public virtual decimal Result { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Формула
        /// </summary>
        public virtual CSFormula CSFormula { get; set; }

        /// <summary>
        /// Жилой дом, по которому запрашивается выписка
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Помещение, по которому запрашивается выписка
        /// </summary>
        public virtual Room Room { get; set; }

        /// <summary>
        ///РАсчет на дату
        /// </summary>
        public virtual DateTime? CalcDate { get; set; }

        ///// <summary>
        ///// Список переменных для расчета
        ///// </summary>
        //public virtual IList<CSCalculationRow> CalculatedVariables => this.calculatedVariables;
    }
}