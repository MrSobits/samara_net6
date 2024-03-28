namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь <see cref="DesignAssignment"/>(Задание на проектирование) и <see cref="SpecialTypeWorkCr"/>(Вид работ)
    /// </summary>
    public class SpecialDesignAssignmentTypeWorkCr : BaseImportableEntity
    {
        /// <summary>
        /// Задание на проектирование
        /// </summary>
        public virtual SpecialDesignAssignment DesignAssignment { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual SpecialTypeWorkCr TypeWorkCr { get; set; }
    }
}