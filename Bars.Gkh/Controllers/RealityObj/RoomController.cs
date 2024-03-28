namespace Bars.Gkh.Controllers.RealityObj
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class RoomController : FileStorageDataController<Room>
    {
        public ActionResult GetLastRoomNumber(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("realtyId");
            var roomId = baseParams.Params.GetAsId("RoomId");

            var roomNumList = this.Container.ResolveDomain<Room>().GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Where(x => x.RoomNum != null)
                .Select(x => x.RoomNum)
                .ToHashSet();

            int roomNum = 999;

            if (roomNumList.Any())
            {
                for (var i = 999; i > 0; i--)
                {
                    if (!roomNumList.Contains(i.ToString()))
                    {
                        roomNum = i;
                        break;
                    }
                }
            }

            return new JsonNetResult(roomNum);
        }

        /// <summary>
        /// Проставить подъезд
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult SetEntrance(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRoomService>();

            using (this.Container.Using(service))
            {
                var result = service.SetEntrance(baseParams);
                return result.ToJsonResult();
            }
        }

        /// <summary>
        /// Получить адреса помещений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListRoomAddress(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRoomService>();

            using (this.Container.Using(service))
            {
                var result = service.ListRoomAddress(baseParams);
                return result.ToJsonResult();
            }
        }
    }
}