/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     /// <summary>
///     /// Маппинг сущности "Страховой случай"
///     /// </summary>
///     public class BelayManOrgActivityPolicyInsuredEventMap : BaseGkhEntityMap<Entities.BelayPolicyEvent>
///     {
///         public BelayManOrgActivityPolicyInsuredEventMap() : base("GKH_BELAY_POLICY_EVENT")
///         {
///             Map(x => x.EventDate, "EVENT_DATE");
///             Map(x => x.Description, "DESCRIPTION").Length(300);
/// 
///             References(x => x.BelayPolicy, "BELAY_POLICY_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileInfo, "FILE_INFO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Страховой случай"</summary>
    public class BelayPolicyEventMap : BaseImportableEntityMap<BelayPolicyEvent>
    {
        
        public BelayPolicyEventMap() : 
                base("Страховой случай", "GKH_BELAY_POLICY_EVENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.EventDate, "Дата наступления страхового случая").Column("EVENT_DATE");
            Property(x => x.Description, "Описание страхового случая").Column("DESCRIPTION").Length(300);
            Reference(x => x.BelayPolicy, "Страховой полис").Column("BELAY_POLICY_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
