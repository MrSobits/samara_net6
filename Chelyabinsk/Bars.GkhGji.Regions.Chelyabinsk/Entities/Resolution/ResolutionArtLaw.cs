namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System;

    /// <summary>
    /// Изменение в статье закона постановления ГЖИ
    /// </summary>
    public class ResolutionArtLaw : BaseGkhEntity
    {
        /// <summary>
        /// Постановление
        /// </summary>
        public virtual Resolution Resolution { get; set; }

        /// <summary>
        /// Дата изменения статьи закона
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLawGji { get; set; }
    }
}