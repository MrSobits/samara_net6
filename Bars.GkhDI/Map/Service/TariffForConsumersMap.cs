/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
///     using Enums;
/// 
///     public class TariffForConsumersMap : BaseGkhEntityMap<TariffForConsumers>
///     {
///         public TariffForConsumersMap()
///             : base("DI_TARIFF_FCONSUMERS")
///         {
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.TariffIsSetFor, "TARIFF_IS_SET_FOR").Not.Nullable().CustomType<TariffIsSetForDi>();
///             Map(x => x.OrganizationSetTariff, "ORGANIZATION_SET_TARIFF");
///             Map(x => x.TypeOrganSetTariffDi, "TYPE_ORGAN_SET_TARIFF").Not.Nullable().CustomType<TypeOrganSetTariffDi>();
/// 
///             Map(x => x.Cost, "COST");
///             Map(x => x.CostNight, "COST_NIGHT");
/// 
///             References(x => x.BaseService, "BASE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.TariffForConsumers"</summary>
    public class TariffForConsumersMap : BaseImportableEntityMap<TariffForConsumers>
    {
        
        public TariffForConsumersMap() : 
                base("Bars.GkhDi.Entities.TariffForConsumers", "DI_TARIFF_FCONSUMERS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.TariffIsSetFor, "TariffIsSetFor").Column("TARIFF_IS_SET_FOR").NotNull();
            Property(x => x.OrganizationSetTariff, "OrganizationSetTariff").Column("ORGANIZATION_SET_TARIFF");
            Property(x => x.TypeOrganSetTariffDi, "TypeOrganSetTariffDi").Column("TYPE_ORGAN_SET_TARIFF").NotNull();
            Property(x => x.Cost, "Cost").Column("COST");
            Property(x => x.CostNight, "CostNight").Column("COST_NIGHT");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
