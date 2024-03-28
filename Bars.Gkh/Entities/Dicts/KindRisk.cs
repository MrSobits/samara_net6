namespace Bars.Gkh.Entities
{
    using Bars.Gkh.ImportExport;

    /// <summary>
    /// Виды рисков
    /// </summary>
    public class KindRisk : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}