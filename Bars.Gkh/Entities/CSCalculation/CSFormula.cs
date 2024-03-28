namespace Bars.Gkh.Entities
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using Formulas;
    using Enums;

    /// <summary>
    /// Формула расчета платы ЖКУ
    /// </summary>
    public class CSFormula : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Формула расчета
        /// </summary>
        public virtual string Formula { get; set; }

        /// <summary>
        /// Параметры формулы
        /// </summary>
        public virtual List<FormulaParameterMeta> FormulaParameters { get;set; } 
    }
}