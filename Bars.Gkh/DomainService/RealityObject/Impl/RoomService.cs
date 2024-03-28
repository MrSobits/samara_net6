namespace Bars.Gkh.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;

    using Castle.Windsor;
    using B4.Utils;

    /// <summary>
    /// </summary>
    public class RoomService : IRoomService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<Room> roomDomain;
        private readonly IDomainService<Entrance> entranceDomain;

        public RoomService(IWindsorContainer container, IDomainService<Room> roomDomain, IDomainService<Entrance> entranceDomain)
        {
            this.container = container;
            this.roomDomain = roomDomain;
            this.entranceDomain = entranceDomain;
        }

        /// <summary>
        /// Проставить подъезд
        /// </summary>
        public IDataResult SetEntrance(BaseParams baseParams)
        {
            ArgumentChecker.NotNull(baseParams, "baseParams");

            try
            {
                var entranceId = baseParams.Params.GetAsId("entranceId");
                var roomIds = baseParams.Params.GetAs<long[]>("roomIds");

                var entrance = this.entranceDomain.Get(entranceId);

                var rooms = this.roomDomain.GetAll()
                    .Where(x => roomIds.Contains(x.Id))
                    .ToArray();

                this.SetEntrance(entrance, rooms);
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Проставить подъезд
        /// </summary>
        public void SetEntrance(Entrance entrance, Room[] rooms)
        {
            ArgumentChecker.NotNull(entrance, "entrance");
            ArgumentChecker.NotNull(rooms, "rooms");

            if (rooms.Any(x => x.RealityObject.Id != entrance.RealityObject.Id))
            {
                throw new GkhException("Помещение привязано к другому дому");
            }

            this.container.InTransaction(
                () =>
                {
                    foreach (var room in rooms)
                    {
                        room.Entrance = entrance;
                        this.roomDomain.SaveOrUpdate(room);
                    }
                });
        }

        /// <summary>
        /// Получить список адресов помещений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListRoomAddress(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.roomDomain.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address + ", кв. " + x.RoomNum
                            + (x.ChamberNum != "" && x.ChamberNum != null ? ", ком. " + x.ChamberNum : string.Empty),
                    })
                    .Filter(loadParams, this.container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}