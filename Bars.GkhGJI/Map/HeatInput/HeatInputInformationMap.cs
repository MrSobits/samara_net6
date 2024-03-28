/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Информация о подаче тепла"
///     /// </summary>
///     public class HeatInputInformationMap : BaseEntityMap<HeatInputInformation>
///     {
///         public HeatInputInformationMap()
///             : base("GJI_HEATING_INPUT_INFORMATION")
///         {
///             References(x => x.HeatInputPeriod, "HEATINPUTPERIOD_ID");
///             Map(x => x.TypeHeatInputObject, "TYPE_HEAT_OBJ").CustomType<TypeHeatInputObject>();
///             Map(x => x.Count, "HEAT_COUNT");
///             Map(x => x.CentralHeating, "HEAT_CENTRAL");
///             Map(x => x.IndividualHeating, "HEAT_INDIVID");
///             Map(x => x.Percent, "HEAT_PERCENT");
///             Map(x => x.NoHeating, "HEAT_NOHEATING");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.HeatInputInformation"</summary>
    public class HeatInputInformationMap : BaseEntityMap<HeatInputInformation>
    {
        
        public HeatInputInformationMap() : 
                base("Bars.GkhGji.Entities.HeatInputInformation", "GJI_HEATING_INPUT_INFORMATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeHeatInputObject, "TypeHeatInputObject").Column("TYPE_HEAT_OBJ");
            Property(x => x.Count, "Count").Column("HEAT_COUNT");
            Property(x => x.CentralHeating, "CentralHeating").Column("HEAT_CENTRAL");
            Property(x => x.IndividualHeating, "IndividualHeating").Column("HEAT_INDIVID");
            Property(x => x.Percent, "Percent").Column("HEAT_PERCENT");
            Property(x => x.NoHeating, "NoHeating").Column("HEAT_NOHEATING");
            Reference(x => x.HeatInputPeriod, "HeatInputPeriod").Column("HEATINPUTPERIOD_ID");
        }
    }
}
