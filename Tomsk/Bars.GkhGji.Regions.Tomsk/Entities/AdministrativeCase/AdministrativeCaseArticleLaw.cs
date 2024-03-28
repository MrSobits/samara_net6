namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Статьи закона административного дела
    /// </summary>
    public class AdministrativeCaseArticleLaw : BaseEntity
    {
        /// <summary>
        /// Дело
        /// </summary>
        public virtual AdministrativeCase AdministrativeCase { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }
    }
}