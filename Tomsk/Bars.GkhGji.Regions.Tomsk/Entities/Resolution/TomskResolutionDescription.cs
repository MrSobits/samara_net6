namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.B4.DataAccess;

    public class TomskResolutionDescription : BaseEntity
    {
        public virtual TomskResolution Resolution { get; set; }

        public virtual byte[] ResolutionText { get; set; }

        public virtual byte[] PetitionText { get; set; }

        public virtual byte[] ExplanationText { get; set; }
    }
}