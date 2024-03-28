/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.BoilerRooms
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities.BoilerRooms;
/// 
///     public class BoilerRoomMap : BaseEntityMap<BoilerRoom>
///     {
///         public BoilerRoomMap() : base("GJI_BOILER_ROOM")
///         {
///             References(x => x.Address, "ADDRESS_ID");
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.BoilerRooms
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.BoilerRooms;
    
    
    /// <summary>Маппинг для "Котельная"</summary>
    public class BoilerRoomMap : BaseEntityMap<BoilerRoom>
    {
        
        public BoilerRoomMap() : 
                base("Котельная", "GJI_BOILER_ROOM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Address, "Адрес котельной").Column("ADDRESS_ID");
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID");
        }
    }
}
