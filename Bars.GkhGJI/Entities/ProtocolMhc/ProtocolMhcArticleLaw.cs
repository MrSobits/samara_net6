namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи закона в протокола МЖК
    /// </summary>
    public class ProtocolMhcArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Протокол МЖК
        /// </summary>
        public virtual ProtocolMhc ProtocolMhc { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}