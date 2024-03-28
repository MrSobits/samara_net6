namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи закона в протоколе МВД
    /// </summary>
    public class ProtocolMvdArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Протокол МВД
        /// </summary>
        public virtual ProtocolMvd ProtocolMvd { get; set; }

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