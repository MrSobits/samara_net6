/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map.ChangeTracker
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities.ChangeTracker;
/// 
///     public class ChangedManOrgMap : PersistentObjectMap<ChangedManOrg>
///     {
///         public ChangedManOrgMap()
///             : base("RFRM_CHANGED_MAN_ORG")
///         {
///             this.References(x => x.ManagingOrganization, "MAN_ORG_ID", ReferenceMapConfig.NotNull);
///             this.References(x => x.PeriodDi, "PERIOD_DI_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map.ChangeTracker
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities.ChangeTracker;
    
    
    /// <summary>Маппинг для "Изменение УО"</summary>
    public class ChangedManOrgMap : PersistentObjectMap<ChangedManOrg>
    {
        
        public ChangedManOrgMap() : 
                base("Изменение УО", "RFRM_CHANGED_MAN_ORG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ManagingOrganization, "УО").Column("MAN_ORG_ID").NotNull();
            Reference(x => x.PeriodDi, "Период раскрытия. Может быть null. В этом случае подразумевается изменение по все" +
                    "м активным периодам.").Column("PERIOD_DI_ID");
        }
    }
}
