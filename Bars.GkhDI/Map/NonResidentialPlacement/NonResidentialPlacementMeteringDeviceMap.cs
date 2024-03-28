/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Приборы учета сведения об использование нежилых помещений"
///     /// </summary>
///     public class NonResidentialPlacementMeteringDeviceMap : BaseGkhEntityMap<NonResidentialPlacementMeteringDevice>
///     {
///         public NonResidentialPlacementMeteringDeviceMap(): base("DI_DISINFO_RONONRESP_METR")
///         {
///             References(x => x.NonResidentialPlacement, "DI_NONRESPLACE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.MeteringDevice, "METERING_DEVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.NonResidentialPlacementMeteringDevice"</summary>
    public class NonResidentialPlacementMeteringDeviceMap : BaseImportableEntityMap<NonResidentialPlacementMeteringDevice>
    {
        
        public NonResidentialPlacementMeteringDeviceMap() : 
                base("Bars.GkhDi.Entities.NonResidentialPlacementMeteringDevice", "DI_DISINFO_RONONRESP_METR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.NonResidentialPlacement, "NonResidentialPlacement").Column("DI_NONRESPLACE_ID").NotNull().Fetch();
            Reference(x => x.MeteringDevice, "MeteringDevice").Column("METERING_DEVICE_ID").NotNull().Fetch();
        }
    }
}
