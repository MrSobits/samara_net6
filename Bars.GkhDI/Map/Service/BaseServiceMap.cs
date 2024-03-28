/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Enums;
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Enums;
/// 
///     using Entities;
/// 
///     public class BaseServiceMap : BaseGkhEntityMap<BaseService>
///     {
///         public BaseServiceMap()
///             : base("DI_BASE_SERVICE")
///         {
///             Map(x => x.Profit, "PROFIT");
///             Map(x => x.TariffForConsumers, "TARIFF");
///             Map(x => x.TariffIsSetForDi, "TARIFF_SET_FOR").CustomType<TariffIsSetForDi>().Not.Nullable();
///             Map(x => x.DateStartTariff, "DATE_START_TARIFF");
/// 
///             Map(x => x.ScheduledPreventiveMaintanance, "SCHEDULE_PREVENT_JOB").Not.Nullable().CustomType<YesNoNotSet>();
/// 
///             References(x => x.TemplateService, "TEMPLATE_SERVICE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Provider, "PROVIDER_ID").Fetch.Join();
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.BaseService"</summary>
    public class BaseServiceMap : BaseImportableEntityMap<BaseService>
    {
        
        public BaseServiceMap() : 
                base("Bars.GkhDi.Entities.BaseService", "DI_BASE_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Profit, "Profit").Column("PROFIT");
            Property(x => x.TariffForConsumers, "TariffForConsumers").Column("TARIFF");
            Property(x => x.TariffIsSetForDi, "TariffIsSetForDi").Column("TARIFF_SET_FOR").NotNull();
            Property(x => x.DateStartTariff, "DateStartTariff").Column("DATE_START_TARIFF");
            Property(x => x.ScheduledPreventiveMaintanance, "ScheduledPreventiveMaintanance").Column("SCHEDULE_PREVENT_JOB").NotNull();
            Reference(x => x.TemplateService, "TemplateService").Column("TEMPLATE_SERVICE_ID").NotNull().Fetch();
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            Reference(x => x.Provider, "Provider").Column("PROVIDER_ID").Fetch();
            Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
