namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Статьи закона в протоколе ГЖИ
    /// </summary>
    public class ProtocolArticleLaw : BaseGkhEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Протокол
        /// </summary>
        public virtual Protocol Protocol { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLaw { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Идентификатор ЕРКНМ
        /// </summary>
        public virtual string ErknmGuid { get; set; }
    }
}