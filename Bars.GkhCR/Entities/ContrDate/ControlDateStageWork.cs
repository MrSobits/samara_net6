namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Этапы работ контрольных сроков вида работ
    /// </summary>
    public class ControlDateStageWork : BaseImportableEntity
    {
        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual ControlDate ControlDate { get; set; }

        /// <summary>
        /// Этапы работ
        /// </summary>
        public virtual StageWorkCr StageWork { get; set; }
    }
}
