namespace Bars.Gkh.Repair.Entities
{
    using B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Объект текущего ремонта
    /// </summary>
    public class RepairObject : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Программа
        /// </summary>
        public virtual RepairProgram RepairProgram { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Документ-основание для проведения работ
        /// </summary>
        public virtual FileInfo ReasonDocument { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Comment { get; set; }
    }
}