namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь <see cref="DesignAssignment"/>(Задание на проектирование) и <see cref="TypeWorkCr"/>(Вид работ)
    /// </summary>
    public class DesignAssignmentTypeWorkCr : BaseImportableEntity
    {
        /// <summary>
        /// Задание на проектирование
        /// </summary>
        public virtual DesignAssignment DesignAssignment { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }
    }
}