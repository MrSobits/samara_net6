namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность справочника шаблонов
    /// </summary>
    public class GisGmpPatternDict : BaseEntity
    {
        /// <summary>
        /// Наименование шаблона
        /// </summary>
        public virtual string PatternName { get; set; }

        /// <summary>
        /// Код шаблона
        /// </summary>
        public virtual string PatternCode { get; set; }

        /// <summary>
        /// Актуальность
        /// </summary>
        public virtual bool Relevance { get; set; }
    }
}