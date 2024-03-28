/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
/// 
///     public class TariffForRSOMap : BaseGkhEntityMap<TariffForRso>
///     {
///         public TariffForRSOMap()
///             : base("DI_TARIFF_FRSO")
///         {
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///             Map(x => x.NumberNormativeLegalAct, "NUMBER_NORMATIVE_LEGAL_ACT").Length(300);
///             Map(x => x.DateNormativeLegalAct, "DATE_NORMATIVE_LEGAL_ACT");
///             Map(x => x.OrganizationSetTariff, "ORGANIZATION_SET_TARIFF").Length(300);
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
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.TariffForRso"</summary>
    public class TariffForRsoMap : BaseImportableEntityMap<TariffForRso>
    {
        
        public TariffForRsoMap() : 
                base("Bars.GkhDi.Entities.TariffForRso", "DI_TARIFF_FRSO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
            Property(x => x.NumberNormativeLegalAct, "NumberNormativeLegalAct").Column("NUMBER_NORMATIVE_LEGAL_ACT").Length(300);
            Property(x => x.DateNormativeLegalAct, "DateNormativeLegalAct").Column("DATE_NORMATIVE_LEGAL_ACT");
            Property(x => x.OrganizationSetTariff, "OrganizationSetTariff").Column("ORGANIZATION_SET_TARIFF").Length(300);
            Property(x => x.Cost, "Cost").Column("COST");
            Property(x => x.CostNight, "CostNight").Column("COST_NIGHT");
            Reference(x => x.BaseService, "BaseService").Column("BASE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
