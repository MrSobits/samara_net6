namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class MKDLicRequestInspectorMap : BaseEntityMap<MKDLicRequestInspector>
    {
        
        public MKDLicRequestInspectorMap() : 
                base("Приложение", "GJI_MKD_LIC_STATEMENT_INSPECTOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.MKDLicRequest, "MKDLicRequest").Column("REQUEST_ID").NotNull().Fetch();
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID").NotNull().Fetch();
        }
    }
}
