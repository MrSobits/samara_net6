/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class ProtocolActivityDirectionMap : BaseEntityMap<ProtocolActivityDirection>
///     {
///         public ProtocolActivityDirectionMap()
///             : base("GJI_PROT_ACTIV_DIRECT")
///         {
///             References(x => x.ActivityDirection, "ACTIVEDIRECT_ID", ReferenceMapConfig.NotNull);
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ProtocolActivityDirection"</summary>
    public class ProtocolActivityDirectionMap : BaseEntityMap<ProtocolActivityDirection>
    {
        
        public ProtocolActivityDirectionMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ProtocolActivityDirection", "GJI_PROT_ACTIV_DIRECT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ActivityDirection, "ActivityDirection").Column("ACTIVEDIRECT_ID").NotNull();
            this.Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
        }
    }
}
