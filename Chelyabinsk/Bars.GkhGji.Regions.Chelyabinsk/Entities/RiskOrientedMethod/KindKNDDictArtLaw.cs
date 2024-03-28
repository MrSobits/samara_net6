namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Enums;


    /// <summary>
    /// Статьи закона по видам КНД
    /// </summary>
    public class KindKNDDictArtLaw : BaseGkhEntity
    {
        /// <summary>
        /// Вид КНД
        /// </summary>
        public virtual KindKNDDict KindKNDDict { get; set; }

        /// <summary>
        /// Статья закона
        /// </summary>
        public virtual ArticleLawGji ArticleLawGji { get; set; }

        /// <summary>
        /// Коэффициент
        /// </summary>
        public virtual Koefficients Koefficients { get; set; }


    }
}