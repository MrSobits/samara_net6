using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.Entities;

namespace Bars.Gkh.LogMap
{
    using B4.Utils;

    public class RoomLogMap : AuditLogMap<Room>
    {
        public RoomLogMap()
        {
            Name("Помещение");

            Description(x => string.Format("{0}, {1}", x.RealityObject.Return(y => y.Address), x.RoomNum));

            MapProperty(x => x.RoomNum, "RoomNum", "Номер помещения");
            MapProperty(x => x.Area, "Address", "Общая площадь");
            MapProperty(x => x.LivingArea, "LivingArea", "Жилая площадь");
            MapProperty(x => x.Type, "Type", "Тип помещения", x => x.Return(y => y.GetEnumMeta().Display));
            MapProperty(x => x.EntranceNum, "EntranceNum", "Номер подъезда");
            MapProperty(x => x.Floor, "Floor", "Этаж");
            MapProperty(x => x.RoomsCount, "RoomsCount", "Количество комнат");
            MapProperty(x => x.OwnershipType, "OwnershipType", "Тип собственности");
        }
    }
}
