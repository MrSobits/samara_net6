namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи в постановлении Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статья
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public virtual ResolutionRospotrebnadzor Resolution { get; set; }
    }
}