namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.GkhGji.Entities;

    public class TomskArticleLawGji : ArticleLawGji
    {
        public virtual string PhysPersonPenalty { get; set; }

        public virtual string JurPersonPenalty { get; set; }

        public virtual string OffPersonPenalty { get; set; }
    }
}