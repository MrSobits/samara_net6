namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using Formulas;
    using Enums;

    /// <summary>
    /// Госпошлина
    /// </summary>
    public class StateDuty : BaseEntity
    {
        /// <summary>
        /// Тип суда
        /// </summary>
        public virtual CourtType CourtType { get; set; }

        /// <summary>
        /// Формула расчтеа пошлины
        /// </summary>
        public virtual string Formula { get; set; }

        /// <summary>
        /// Параметры формулы
        /// </summary>
        public virtual List<FormulaParameterMeta> FormulaParameters { get;set; } 
    }
}