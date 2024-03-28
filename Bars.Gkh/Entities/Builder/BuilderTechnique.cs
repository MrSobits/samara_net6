namespace Bars.Gkh.Entities
{
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Техника, Инструменты подрядчика
    /// </summary>
    public class BuilderTechnique : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}
