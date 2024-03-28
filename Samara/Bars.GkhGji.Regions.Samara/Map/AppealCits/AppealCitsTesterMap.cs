/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Samara.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Samara.Entities;
/// 
///     public class AppealCitsTesterMap : BaseEntityMap<AppealCitsTester>
///     {
///         public AppealCitsTesterMap()
///             : base("GJI_SAM_APPCITS_TESTER")
///         {
///             References(x => x.AppealCits, "APPEALCIT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Tester, "TESTER_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Samara.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Samara.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Samara.Entities.AppealCitsTester"</summary>
    public class AppealCitsTesterMap : BaseEntityMap<AppealCitsTester>
    {
        
        public AppealCitsTesterMap() : 
                base("Bars.GkhGji.Regions.Samara.Entities.AppealCitsTester", "GJI_SAM_APPCITS_TESTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AppealCits, "AppealCits").Column("APPEALCIT_ID").NotNull().Fetch();
            Reference(x => x.Tester, "Tester").Column("TESTER_ID").NotNull().Fetch();
        }
    }
}
