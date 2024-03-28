namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class MKDLicRequestRealityObjectMap : BaseEntityMap<MKDLicRequestRealityObject>
    {
        
        public MKDLicRequestRealityObjectMap() : 
                base("Приложение", "GJI_MKD_LIC_STATEMENT_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.MKDLicRequest, "MKDLicRequest").Column("REQUEST_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();           
        }
    }
}
