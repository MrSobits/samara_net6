namespace Bars.Gkh.Repair.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Муниципальное образование программы текущего ремонта
    /// </summary>
    public class RepairProgramMunicipality : BaseEntity
    {
        /// <summary>
        /// Программа текущего ремонта
        /// </summary>
        public virtual RepairProgram RepairProgram { get; set; }
        
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
