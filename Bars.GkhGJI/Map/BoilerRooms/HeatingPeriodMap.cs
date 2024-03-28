/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.BoilerRooms
/// {
///     using System;
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.BoilerRooms;
/// 
///     public class HeatingPeriodMap : BaseEntityMap<HeatingPeriod>
///     {
///         public HeatingPeriodMap() : base("GJI_BOILER_HEATING")
///         {
///             Map(x => x.Start, "START_TIME", true, DateTime.MinValue);
///             Map(x => x.End, "END_TIME", false, DateTime.MaxValue);
///             References(x => x.BoilerRoom, "BOILER_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.BoilerRooms
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.BoilerRooms;
    using System;
    
    
    /// <summary>Маппинг для "Период подачи тепла в котельной"</summary>
    public class HeatingPeriodMap : BaseEntityMap<HeatingPeriod>
    {
        
        public HeatingPeriodMap() : 
                base("Период подачи тепла в котельной", "GJI_BOILER_HEATING")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BoilerRoom, "Котельная").Column("BOILER_ID");
            Property(x => x.Start, "Начала периода").Column("START_TIME").DefaultValue(DateTime.MinValue).NotNull();
            Property(x => x.End, "Конец периода").Column("END_TIME").DefaultValue(DateTime.MaxValue);
        }
    }
}
