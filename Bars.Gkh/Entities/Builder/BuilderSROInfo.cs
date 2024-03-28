namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Сведения об участии в СРО
    /// </summary>
    public class BuilderSroInfo : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Описание работ
        /// </summary>
        public virtual FileInfo DescriptionWork { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }
    }
}
