/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Страхование деятельности управляющей организации"
///     /// </summary>
///     public class BelayManOrgActivityMap : BaseGkhEntityMap<Entities.BelayManOrgActivity>
///     {
///         public BelayManOrgActivityMap() : base("GKH_BELAY_MORG_ACTIVITY")
///         {
///             References(x => x.ManagingOrganization, "MANORG_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Страхование деятельности управляющей организации"</summary>
    public class BelayManOrgActivityMap : BaseImportableEntityMap<BelayManOrgActivity>
    {
        
        public BelayManOrgActivityMap() : 
                base("Страхование деятельности управляющей организации", "GKH_BELAY_MORG_ACTIVITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
        }
    }
}
