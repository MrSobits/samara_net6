namespace Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType
{
    using B4.DataAccess;

    /// <summary>
    /// Тип дома
    /// </summary>
    public class GisRealEstateType : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual GisRealEstateTypeGroup Group { get; set; }
    }
}