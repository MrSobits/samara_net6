namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Статья устава деятельности ТСЖ
    /// </summary>
    public class ActivityTsjArticle : BaseGkhEntity
    {
        /// <summary>
        /// Устав деятельности ТСЖ
        /// </summary>
        public virtual ActivityTsjStatute ActivityTsjStatute { get; set; }

        /// <summary>
        /// Статья ТСЖ
        /// </summary>
        public virtual ArticleTsj ArticleTsj { get; set; }

        /// <summary>
        /// Отсутствует
        /// </summary>
        public virtual bool IsNone { get; set; }

        /// <summary>
        /// Пункт устава
        /// </summary>
        public virtual string Paragraph { get; set; }

        /// <summary>
        /// Пункт устава
        /// </summary>
        public virtual TypeState TypeState { get; set; }
    }
}