namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using Bars.B4.DataAccess;

    public class ResolutionLongDescription : BaseEntity
    {
        public virtual GkhGji.Entities.Resolution Resolution { get; set; }

        public virtual byte[] Description { get; set; }
    }
}