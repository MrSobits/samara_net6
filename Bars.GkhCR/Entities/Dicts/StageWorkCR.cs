namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Этапы работ капитального ремонта
    /// </summary>
    public class StageWorkCr : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование этапа
        /// </summary>
        public virtual string Name { get; set; }
    }
}
