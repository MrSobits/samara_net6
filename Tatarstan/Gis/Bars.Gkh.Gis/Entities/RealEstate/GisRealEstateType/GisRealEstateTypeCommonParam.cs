namespace Bars.Gkh.Gis.Entities.RealEstate.GisRealEstateType
{
    using B4.DataAccess;

    /// <summary>
    /// Общие параметры типа дома
    /// </summary>
    public class GisRealEstateTypeCommonParam : BaseEntity
    {
        public virtual string CommonParamCode { get; set; }

        public virtual GisRealEstateType RealEstateType { get; set; }

        public virtual string Min { get; set; }

        public virtual string Max { get; set; }

        public virtual string PrecisionValue { get; set; }
    }
}