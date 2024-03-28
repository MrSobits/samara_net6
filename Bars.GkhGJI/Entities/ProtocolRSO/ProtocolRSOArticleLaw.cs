namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи закона в протоколе прокуратуры
    /// </summary>
    public class ProtocolRSOArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// Протокол прокуратуры
        /// </summary>
        public virtual ProtocolRSO ProtocolRSO { get; set; }

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