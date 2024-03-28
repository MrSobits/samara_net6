namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Статьи закона в постановлении прокуратуры ГЖИ
    /// </summary>
    public class ResolProsArticleLaw : BaseGkhEntity
    {
        /// <summary>
        /// постановление прокуратуры
        /// </summary>
        public virtual ResolPros ResolPros { get; set; }

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