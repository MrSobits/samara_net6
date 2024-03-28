namespace Bars.Gkh.Regions.Samara.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class ApartInfoOwnerCode : BaseEntity
    {
        public virtual RealityObjectApartInfo RealityObjectApartInfo { get; set; }

        public virtual string OwnerCode { get; set; }
    }
}