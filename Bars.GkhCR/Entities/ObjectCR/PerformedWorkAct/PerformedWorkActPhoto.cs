namespace Bars.GkhCr.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Фото в акте выполненных работ
    /// </summary>
    public class PerformedWorkActPhoto : BaseEntity
    {
        /// <summary>
        /// Акт выполненных работ
        /// </summary>
        public virtual PerformedWorkAct PerformedWorkAct { get; set; }

        /// <summary>
        /// Фото
        /// </summary>
        public virtual FileInfo Photo { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Discription { get; set; }

        /// <summary>
        /// Тип фото (до/после)
        /// </summary>
        public virtual PerfWorkActPhotoType PhotoType { get; set; }
    }
}
