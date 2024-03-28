/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Enums;
///     using Entities;
///     using FluentNHibernate.Mapping;
/// 
///     public class CapRepairServiceMap : SubclassMap<CapRepairService>
///     {
///         public CapRepairServiceMap()
///         {
///             Table("DI_CAP_REP_SERVICE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.TypeOfProvisionService, "TYPE_OF_PROVISION_SERVICE").Not.Nullable().CustomType<TypeOfProvisionServiceDi>();
///             Map(x => x.RegionalFund, "REGIONAL_FUND").CustomType<RegionalFundDi>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.CapRepairService"</summary>
    public class CapRepairServiceMap : JoinedSubClassMap<CapRepairService>
    {
        
        public CapRepairServiceMap() : 
                base("Bars.GkhDi.Entities.CapRepairService", "DI_CAP_REP_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeOfProvisionService, "TypeOfProvisionService").Column("TYPE_OF_PROVISION_SERVICE").NotNull();
            Property(x => x.RegionalFund, "RegionalFund").Column("REGIONAL_FUND");
        }
    }
}
