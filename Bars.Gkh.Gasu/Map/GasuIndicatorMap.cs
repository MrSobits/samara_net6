/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Gasu.Entities;
/// 
///     public class GasuIndicatorMap : BaseEntityMap<GasuIndicator>
///     {
///         public GasuIndicatorMap()
///             : base("GASU_INDICATOR")
///         {
///             Map(x => x.Name, "NAME");
///             Map(x => x.Code, "CODE");
///             Map(x => x.Periodicity, "PERIODICITY", true, (object)10);
///             Map(x => x.EbirModule, "EBIR_MODULE", true, (object)10);
/// 
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Gasu.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Gasu.Entities;
    using Bars.Gkh.Gasu.Enums;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Gasu.Entities.GasuIndicator"</summary>
    public class GasuIndicatorMap : BaseEntityMap<GasuIndicator>
    {
        
        public GasuIndicatorMap() : 
                base("Bars.Gkh.Gasu.Entities.GasuIndicator", "GASU_INDICATOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(250);
            Property(x => x.Code, "Code").Column("CODE").Length(250);
            Reference(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE_ID").Fetch();
            Property(x => x.Periodicity, "Periodicity").Column("PERIODICITY").DefaultValue(0).NotNull();
            Property(x => x.EbirModule, "EbirModule").Column("EBIR_MODULE").DefaultValue(0).NotNull();
        }
    }
}
