/// <mapping-converter-backup>
/// using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
/// 
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     public class ActionsMap : BaseGkhEntityMap<Actions>
///     {
///         public ActionsMap()
///             : base("DI_ADMIN_RESP_ACTION")
///         {
///             Map(x => x.Action, "ACTION").Length(2000);
/// 
///             References(x => x.AdminResp, "ADMIN_RESP_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.Actions"</summary>
    public class ActionsMap : BaseImportableEntityMap<Actions>
    {
        
        public ActionsMap() : 
                base("Bars.GkhDi.Entities.Actions", "DI_ADMIN_RESP_ACTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Action, "Action").Column("ACTION").Length(2000);
            Reference(x => x.AdminResp, "AdminResp").Column("ADMIN_RESP_ID").NotNull().Fetch();
        }
    }
}
