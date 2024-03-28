namespace Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType
{
    using B4.DataAccess;

    /// <summary>
    /// Общие параметры типа дома
    /// </summary>
    public class GisRealEstateTypeIndicator : BaseEntity
    {
        public virtual RealEstateIndicator RealEstateIndicator { get; set; }

        public virtual GisRealEstateType RealEstateType { get; set; }

        public virtual string Min { get; set; }

        public virtual string Max { get; set; }

        public virtual string PrecisionValue { get; set; }
    }
}