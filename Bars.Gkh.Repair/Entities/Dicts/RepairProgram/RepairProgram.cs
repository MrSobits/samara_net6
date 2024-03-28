namespace Bars.Gkh.Repair.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Enums;

    /// <summary>
    /// Программа текущего ремонта
    /// </summary>
    public class RepairProgram : BaseEntity
    {
        /// <summary>
        /// Наименование программы
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Видимость
        /// </summary>
        public virtual TypeVisibilityProgramRepair TypeVisibilityProgramRepair { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public virtual TypeProgramRepairState TypeProgramRepairState { get; set; }
    }
}
