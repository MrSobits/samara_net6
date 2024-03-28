/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.BoilerRooms
/// {
///     using System;
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Entities.BoilerRooms;
/// 
///     public class UnactivePeriodMap : BaseEntityMap<UnactivePeriod>
///     {
///         public UnactivePeriodMap()
///             : base("GJI_BOILER_UNACTIVE")
///         {
///             Map(x => x.Start, "START_TIME", false, DateTime.MinValue);
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
    
    
    /// <summary>Маппинг для "Период неактивности котельной"</summary>
    public class UnactivePeriodMap : BaseEntityMap<UnactivePeriod>
    {
        
        public UnactivePeriodMap() : 
                base("Период неактивности котельной", "GJI_BOILER_UNACTIVE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BoilerRoom, "Котельная").Column("BOILER_ID");
            Property(x => x.Start, "Начало периода").Column("START_TIME").DefaultValue(DateTime.MinValue);
            Property(x => x.End, "Конец периода").Column("END_TIME").DefaultValue(DateTime.MaxValue);
        }
    }
}
