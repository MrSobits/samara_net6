/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
/// 
///     public class OtherServiceMap : BaseGkhEntityMap<OtherService>
///     {
///         public OtherServiceMap()
///             : base("DI_OTHER_SERVICE")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.UnitMeasure, "UNIT_MEASURE").Length(300);
///             Map(x => x.Tariff, "TARIFF");
///             Map(x => x.Provider, "PROVIDER").Length(300);
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.OtherService"</summary>
    public class OtherServiceMap : BaseImportableEntityMap<OtherService>
    {
        
        public OtherServiceMap() : 
                base("Bars.GkhDi.Entities.OtherService", "DI_OTHER_SERVICE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Name").Column("NAME").Length(300);
            this.Property(x => x.Code, "Code").Column("CODE").Length(50);
            this.Property(x => x.UnitMeasureStr, "UnitMeasureStr").Column("UNIT_MEASURE").Length(300);
            this.Property(x => x.Tariff, "Tariff").Column("TARIFF");
            this.Property(x => x.Provider, "Provider").Column("PROVIDER").Length(300);
            this.Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
            this.Reference(x => x.TemplateOtherService, "TemplateOtherService").Column("TEMPLATE_OTHER_SERVICE_ID").Fetch();
            this.Reference(x => x.UnitMeasure, "UnitMeasure").Column("unit_measure_id").Fetch();
        }
    }
}
