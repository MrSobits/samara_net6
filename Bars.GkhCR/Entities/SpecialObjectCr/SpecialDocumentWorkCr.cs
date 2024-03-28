namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Enums;

    using Gkh.Entities;

    /// <summary>
    /// Документ работы объекта КР
    /// </summary>
    public class SpecialDocumentWorkCr : BaseDocument
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual SpecialObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Участник
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual SpecialTypeWorkCr TypeWork { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }
    }
}
