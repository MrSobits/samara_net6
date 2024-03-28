/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Услуга управляющей организации"
///     /// </summary>
///     public class ManagingOrgServiceMap : BaseGkhEntityMap<ManagingOrgService>
///     {
///         public ManagingOrgServiceMap() : base("GKH_MAN_ORG_SERVICE")
///         {
///             Map(x => x.Name, "NAME").Length(250).Not.Nullable();
///             References(x => x.ManagingOrganization, "MAN_ORG_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Услуга управляющей организации"</summary>
    public class ManagingOrgServiceMap : BaseImportableEntityMap<ManagingOrgService>
    {
        
        public ManagingOrgServiceMap() : 
                base("Услуга управляющей организации", "GKH_MAN_ORG_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MAN_ORG_ID").NotNull().Fetch();
            Reference(x => x.Work, "Вид работы").Column("WORK_ID").Fetch();
        }
    }
}
