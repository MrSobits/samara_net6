namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Статья закона требования
    /// </summary>
    public class RequirementArticleLaw : BaseEntity
    {
        /// <summary>
        /// Требование
        /// </summary>
        public virtual Requirement Requirement { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }
    }
}