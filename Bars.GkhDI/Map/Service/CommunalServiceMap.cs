/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Entities;
///     using Enums;
///     using FluentNHibernate.Mapping;
/// 
///     public class CommunalServiceMap : SubclassMap<CommunalService>
///     {
///         public CommunalServiceMap()
///         {
///             Table("DI_COMMUNAL_SERVICE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.VolumePurchasedResources, "VOLUME_PURCHASED_RESOURCES");
///             Map(x => x.PricePurchasedResources, "PRICE_PURCHASED_RESOURCES");
///             Map(x => x.TypeOfProvisionService, "TYPE_OF_PROVISION_SERVICE").Not.Nullable().CustomType<TypeOfProvisionServiceDi>();
/// 
///             Map(x => x.KindElectricitySupply, "KIND_ELECTRICITY_SUPPLY").CustomType<KindElectricitySupplyDi>();
/// 
/// 
///             Map(x => x.ConsumptionNormLivingHouse, "CONS_NORM_LIV_HOUSE").Length(300);
///             Map(x => x.AdditionalInfoLivingHouse, "ADDITIONAL_INFO_LIV_HOUSE").Length(300);
///             Map(x => x.ConsumptionNormHousing, "CONS_NORM_HOUSING").Length(300);
///             Map(x => x.AdditionalInfoHousing, "ADDITIONAL_INFO_HOUSING").Length(300);
/// 
///             References(x => x.UnitMeasureHousing, "UNIT_MEASURE_HOUSING_ID").Fetch.Join();
///             References(x => x.UnitMeasureLivingHouse, "UNIT_MEASURE_LIV_HOUSE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.CommunalService"</summary>
    public class CommunalServiceMap : JoinedSubClassMap<CommunalService>
    {
        
        public CommunalServiceMap() : 
                base("Bars.GkhDi.Entities.CommunalService", "DI_COMMUNAL_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.VolumePurchasedResources, "VolumePurchasedResources").Column("VOLUME_PURCHASED_RESOURCES");
            Property(x => x.PricePurchasedResources, "PricePurchasedResources").Column("PRICE_PURCHASED_RESOURCES");
            Property(x => x.TypeOfProvisionService, "TypeOfProvisionService").Column("TYPE_OF_PROVISION_SERVICE").NotNull();
            Property(x => x.KindElectricitySupply, "KindElectricitySupply").Column("KIND_ELECTRICITY_SUPPLY");
            Property(x => x.ConsumptionNormLivingHouse, "ConsumptionNormLivingHouse").Column("CONS_NORM_LIV_HOUSE");
            Property(x => x.AdditionalInfoLivingHouse, "AdditionalInfoLivingHouse").Column("ADDITIONAL_INFO_LIV_HOUSE").Length(300);
            Property(x => x.ConsumptionNormHousing, "ConsumptionNormHousing").Column("CONS_NORM_HOUSING").Length(300);
            Property(x => x.AdditionalInfoHousing, "AdditionalInfoHousing").Column("ADDITIONAL_INFO_HOUSING").Length(300);
            Reference(x => x.UnitMeasureHousing, "UnitMeasureHousing").Column("UNIT_MEASURE_HOUSING_ID").Fetch();
            Reference(x => x.UnitMeasureLivingHouse, "UnitMeasureLivingHouse").Column("UNIT_MEASURE_LIV_HOUSE_ID").Fetch();
        }
    }
}
