namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="RoomProxy"/>
    /// </summary>
    [Obsolete("Не выгружаем", true)]
    public class RoomSelectorService : BaseProxySelectorService<RoomProxy>
    {
        protected override IDictionary<long, RoomProxy> GetCache()
        {
            var roomRepository = this.Container.Resolve<IRepository<Room>>();

            using (this.Container.Using(roomRepository))
            {
                var premises = roomRepository.GetAll()
                    //.WhereContainsBulked(x => x.RealityObject.Id, roIds)
                    .WhereEmptyString(x => x.ChamberNum)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.RoomNum
                    })
                    .AsEnumerable()
                    .GroupBy(x => $"{x.RoId}|{x.RoomNum}", x => (long?)x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                return roomRepository.GetAll()
                    //.WhereContainsBulked(x => x.RealityObject.Id, roIds)
                    .WhereNotEmptyString(x => x.ChamberNum)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.RoomNum,
                        x.ChamberNum,
                        x.Area,
                        x.RealityObject.CadastralHouseNumber
                    })
                    .AsEnumerable()
                    .Where(x => premises.Get($"{x.RoId}|{x.RoomNum}").HasValue)
                    .Select(x => new RoomProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RoId,
                        PremisesId = premises.Get($"{x.RoId}|{x.RoomNum}"),
                        IsCommunalRoom = false,
                        CadastralHouseNumber = x.CadastralHouseNumber,
                        ChamberNum = x.ChamberNum,
                        Area = x.Area
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}