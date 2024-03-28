namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    
    using Bars.Gkh.Entities;

    /// <summary>
    /// Настройки расчета пени с отсрочкой
    /// </summary>
    public class PenaltiesWithDeferred : BaseImportableEntity
    {
        /// <summary>
        /// Дата начала расчета пени с отсрочкой
        /// </summary>
        public virtual DateTime DateStartCalc { get; set; }

        /// <summary>
        /// Дата окончания расчета пени с отсрочкой
        /// </summary>
        public virtual DateTime DateEndCalc { get; set; }
    }
}