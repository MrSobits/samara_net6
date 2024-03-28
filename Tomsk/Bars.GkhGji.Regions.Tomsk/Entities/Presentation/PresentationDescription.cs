namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.GkhGji.Entities;
    using Bars.B4.DataAccess;

    public class PresentationDescription : BaseEntity
    {
        public virtual Presentation Presentation { get; set; }

        public virtual byte[] DescriptionSet { get; set; }
    }
}