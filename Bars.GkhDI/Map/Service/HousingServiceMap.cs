/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Entities;
///     using Enums;
///     using FluentNHibernate.Mapping;
/// 
///     public class HousingServiceMap : SubclassMap<HousingService>
///     {
///         public HousingServiceMap()
///         {
///             Table("DI_HOUSING_SERVICE");
///             KeyColumn("ID");
///             
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///       
///             Map(x => x.ProtocolNumber, "PROTOCOL_NUMBER").Length(300);
///             Map(x => x.ProtocolFrom, "PROTOCOL_FROM");
///             Map(x => x.Equipment, "EQUIPMENT").CustomType<EquipmentDi>();
///             Map(x => x.TypeOfProvisionService, "TYPE_OF_PROVISION_SERVICE").Not.Nullable().CustomType<TypeOfProvisionServiceDi>();
/// 
///             References(x => x.Periodicity, "PERIODICITY_ID").Fetch.Join();
///             References(x => x.Protocol, "PROTOCOL").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.HousingService"</summary>
    public class HousingServiceMap : JoinedSubClassMap<HousingService>
    {
        
        public HousingServiceMap() : 
                base("Bars.GkhDi.Entities.HousingService", "DI_HOUSING_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ProtocolNumber, "ProtocolNumber").Column("PROTOCOL_NUMBER").Length(300);
            Property(x => x.ProtocolFrom, "ProtocolFrom").Column("PROTOCOL_FROM");
            Property(x => x.Equipment, "Equipment").Column("EQUIPMENT");
            Property(x => x.TypeOfProvisionService, "TypeOfProvisionService").Column("TYPE_OF_PROVISION_SERVICE").NotNull();
            Reference(x => x.Periodicity, "Periodicity").Column("PERIODICITY_ID").Fetch();
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL").Fetch();
        }
    }
}
